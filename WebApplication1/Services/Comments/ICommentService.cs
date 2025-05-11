using ForumBE.DTOs.Comments;
using ForumBE.DTOs.Paginations;

namespace ForumBE.Services.Comments
{
    public interface ICommentService
    {
        Task<PagedList<CommentResponseDto>> GetAllCommentsAsync(PaginationDto input);
        Task<PagedList<CommentResponseDto>> GetAllCommentsByPostAsync(PaginationDto input, int postId);
        Task<CommentResponseDto> GetCommentByIdAsync(int commentId);
        Task<CommentResponseDto> CreateCommentAsync(CommentCreateRequestDto input);
        Task<bool> UpdateCommentAsync(CommentUpdateRequestDto input, int commentId);
        Task<bool> DeleteCommentAsync(int commentId, int userId);
    }
}
