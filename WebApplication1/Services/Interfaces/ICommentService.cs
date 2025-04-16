using ForumBE.DTOs.Comments;

namespace ForumBE.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentResponseDto>> GetAllCommentsAsync();
        Task<CommentResponseDto> GetCommentByIdAsync(int commentId);
        Task<bool> CreateCommentAsync(CommentCreateRequestDto request);
        Task<bool> UpdateCommentAsync(int commentId, CommentUpdateRequestDto request);
        Task<bool> DeleteCommentAsync(int commentId);
    }
}
