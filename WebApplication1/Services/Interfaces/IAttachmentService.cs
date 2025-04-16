using ForumBE.DTOs.Attachments;
using ForumBE.Models;

namespace ForumBE.Services.Interfaces
{
    public interface IAttachmentService
    {
        Task<IEnumerable<AttachmentResponseDto>> GetAllAttachmentAsync();
        Task<AttachmentResponseDto> GetAttachmentByIdAsync(int id);
        Task<bool> CreateAttachmentsAsync(AttachmentCreateRequestDto input);
        Task<bool> DeleteAttachmentAsync(int id);
    }
}
