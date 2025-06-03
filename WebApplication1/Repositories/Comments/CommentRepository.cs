using ForumBE.DTOs.Comments;
using ForumBE.DTOs.Paginations;
using ForumBE.Models;
using ForumBE.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Comments
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> CountLike(int commentId)
        {
            var likes = await _context.Comments
                .Where(c => c.CommentId == commentId)
                .CountAsync();
            return likes;
        }

        public override async Task<Comment> GetByIdAsync(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.User.UserProfile)
                .Include(c => c.Attachments)
                .Include(c => c.Likes)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.CommentId == id);

            return comment;
        }

        public async Task<PagedList<CommentResponseDto>> GetAllCommentsByPost(PaginationDto input, int postId, int? userId)
        {
            // Lấy tất cả comment của bài viết
            var allComments = await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User).ThenInclude(u => u.UserProfile)
                .Include(c => c.Likes)
                .Include(c => c.Attachments) // ✅ Bổ sung Include
                .AsNoTracking()
                .ToListAsync();

            // Lấy comment gốc (không phải reply)
            var parentComments = allComments.Where(c => c.ReplyTo == null).ToList();

            // Sắp xếp comment gốc
            parentComments = input.SortBy?.ToLower() switch
            {
                "oldest" => parentComments.OrderBy(c => c.CreatedAt).ToList(),
                "popular" => parentComments.OrderByDescending(c => c.Likes.Count).ToList(),
                _ => parentComments.OrderByDescending(c => c.CreatedAt).ToList(), // newest
            };

            // Phân trang comment gốc
            var pagedParents = parentComments
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            // Map sang DTO
            var allDtos = allComments.Select(c => new CommentResponseDto
            {
                CommentId = c.CommentId,
                FullName = c.User.FirstName + " " + c.User.LastName,
                UserId = c.User.UserId,
                Content = c.Content,
                LikeCount = c.Likes.Count,
                AvatarUrl = c.User.UserProfile.AvatarUrl,
                FileTypes = c.Attachments.Select(a => a.FileType).ToList(),
                FileUrls = c.Attachments.Select(a => a.FileUrl).ToList(),
                IsDeleted = c.IsDeleted,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Replies = new List<CommentResponseDto>(),
                IsLiked = userId.HasValue && c.Likes.Any(l => l.UserId == userId.Value)
            }).ToList();

            // Tạo dictionary để hỗ trợ gán replies
            var dtoDict = allDtos.ToDictionary(d => d.CommentId);

            // Gán các replies vào comment cha
            foreach (var comment in allComments)
            {
                if (comment.ReplyTo != null && dtoDict.ContainsKey(comment.ReplyTo.Value))
                {
                    dtoDict[comment.ReplyTo.Value].Replies.Add(dtoDict[comment.CommentId]);
                }
            }

            var result = pagedParents.Select(c => dtoDict[c.CommentId]).ToList();

            return new PagedList<CommentResponseDto>(
                result,
                parentComments.Count,
                input.PageNumber,
                input.PageSize
            );
        }


        public override async Task<PagedList<Comment>> GetAllPagesAsync(PaginationDto input)
        {
            var query = _context.Comments
                .Include(c => c.User)
                .Include(c => c.User.UserProfile)
                .Include (c => c.Attachments)
                .Include(c => c.Likes)
                .Include(c => c.Post)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();
            return await PagedList<Comment>.CreateAsync(query, input.PageNumber, input.PageSize);
        }

        public async Task<bool> HasUserLikeComment(int commentId, int userId)
        {
            return await _context.Likes
                .AnyAsync(l => l.CommentId == commentId && l.UserId == userId);
        }
    }
}
