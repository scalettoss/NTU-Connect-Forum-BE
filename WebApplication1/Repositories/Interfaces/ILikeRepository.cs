using ForumBE.DTOs.Likes;
using ForumBE.Models;

namespace ForumBE.Repositories.Interfaces
{
    public interface ILikeRepository : IGenericRepository<Like>
    {
        Task<Like> GetLikesPostByUser(int postId, int userId);
        Task<Like> GetLikesCommentByUser(int commentId, int userId);
        Task<int> GetLikeCountFromPostAsync(int postId);
        Task<int> GetLikeCountFromCommentAsync(int commentId);
    }
}
