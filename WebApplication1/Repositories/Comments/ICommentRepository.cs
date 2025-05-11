using ForumBE.DTOs.Paginations;
using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Comments
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<int> CountLike(int commentId);
        Task<PagedList<Comment>> GetAllCommentsByPost(PaginationDto input, int postId);
        Task<bool> HasUserLikeComment(int commentId, int userId);
    }
   
}
