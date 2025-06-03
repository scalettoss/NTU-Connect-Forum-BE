using ForumBE.DTOs.Comments;
using ForumBE.DTOs.Paginations;
using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Comments
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<int> CountLike(int commentId);
        Task<PagedList<CommentResponseDto>> GetAllCommentsByPost(PaginationDto input, int postId, int? userId);
        Task<bool> HasUserLikeComment(int commentId, int userId);
    }
   
}
