using AutoMapper;
using ForumBE.DTOs.Attachments;
using ForumBE.DTOs.Exception;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Services.Implementations
{
    public class AttachmentService : IAttachmentService
    {

        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;
        private readonly ICommentRepository _commentRepository;

        public AttachmentService(IAttachmentRepository attachmentRepository, IMapper mapper, IUserRepository userRepository,
                                 IPostRepository postRepository, ClaimContext userContextService, ICommentRepository commentRepository)
        {
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _userContextService = userContextService;
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<AttachmentResponseDto>> GetAllAttachmentAsync()
        {
            var list = await _attachmentRepository.GetAllAsync();
            var listMap = _mapper.Map<IEnumerable<AttachmentResponseDto>>(list);

            return listMap;
        }

        public async Task<AttachmentResponseDto> GetAttachmentByIdAsync(int id)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(id);
            if (attachment == null)
            {
                throw new HandleException("Attachment not found", 404);
            }
            var attachmentMap = _mapper.Map<AttachmentResponseDto>(attachment);

            return attachmentMap;
        }

        public async Task<bool> CreateAttachmentsAsync(AttachmentCreateRequestDto attachmentDto)
        {
            if (attachmentDto.PostId == null && attachmentDto.CommentId == null)
            {
                throw new HandleException("Either PostId or CommentId must be provided.", 400);
            }

            if (attachmentDto.PostId != null && attachmentDto.CommentId != null)
            {
                throw new HandleException("Only one of PostId or CommentId should be provided.", 400);
            }

            try
            {
                var userId = _userContextService.GetUserId();
                if (attachmentDto.PostId != null)
                {
                    var post = await _postRepository.GetByIdAsync(attachmentDto.PostId.Value);
                    if (post == null)
                    {
                        throw new HandleException("Post not found.", 404);
                    }
                }

                if (attachmentDto.CommentId != null)
                {
                    var comment = await _commentRepository.GetByIdAsync(attachmentDto.CommentId.Value);
                    if (comment == null)
                    {
                        throw new HandleException("Comment not found.", 404);
                    }
                }
                if (attachmentDto.Files == null || !attachmentDto.Files.Any())
                {
                    throw new HandleException("No files uploaded.", 400);
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx" };
                const long MaxFileSize = 10 * 1024 * 1024; // 10MB
                var dateFolder = DateTime.UtcNow.ToString("yyyyMMdd");
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/public/uploads", dateFolder);

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Danh sách các attachment để lưu vào database
                var attachments = new List<Attachment>();

                // Xử lý từng file
                foreach (var file in attachmentDto.Files)
                {
                    // Kiểm tra file
                    if (file.Length == 0)
                    {
                        continue; // Bỏ qua file rỗng
                    }

                    // Kiểm tra định dạng file
                    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        throw new HandleException($"Invalid file extension for {file.FileName}.", 400);
                    }

                    // Kiểm tra kích thước file
                    if (file.Length > MaxFileSize)
                    {
                        throw new HandleException($"File size exceeds 10MB for {file.FileName}.", 400);
                    }

                    // Tạo tên file ngẫu nhiên
                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Lưu file vật lý
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Tạo đường dẫn public
                    var fileUrl = $"/uploads/{dateFolder}/{uniqueFileName}";

                    // Tính kích thước file
                    var fileSizeInMB = (file.Length / 1024f / 1024f).ToString("0.00") + " MB";

                    // Tạo đối tượng attachment
                    var attachment = new Attachment
                    {
                        PostId = attachmentDto.PostId,
                        CommentId = attachmentDto.CommentId,
                        UserId = userId,
                        FileUrl = fileUrl,
                        FileType = file.ContentType,
                        FileSize = fileSizeInMB,
                        CreatedAt = DateTime.UtcNow,
                    };

                    attachments.Add(attachment);
                }

                // Kiểm tra xem có file nào đã được xử lý không
                if (!attachments.Any())
                {
                    throw new HandleException("No valid files to upload.", 400);
                }

                // Lưu tất cả attachments vào database
                await _attachmentRepository.AddRangeAsync(attachments);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteAttachmentAsync(int attachmentId)
        {
            try
            {
                var userId = _userContextService.GetUserId();
                var userRole = _userContextService.GetUserRoleName();
                var attachment = await _attachmentRepository.GetByIdAsync(attachmentId);

                if (attachment == null)
                {
                    throw new HandleException("Attachment not found.", 404);
                }

                

                try
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/public", attachment.FileUrl.TrimStart('/'));

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch (HandleException ex)
                {
                    throw new HandleException($"Error deleting file: {ex.Message}", ex.Status);
                }

                // Xóa record trong database
                await _attachmentRepository.DeleteAsync(attachment);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
