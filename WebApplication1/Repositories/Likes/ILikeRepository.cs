using ForumBE.Models;
using ForumBE.Repositories.Generics;
using System.Collections;

namespace ForumBE.Repositories.Likes
{
    public interface ILikeRepository : IGenericRepository<Like>
    {
        Task<Like> GetLikesPostByUser(int postId, int userId);
        Task<List<int>> GetLikedCommentIdsByUserAsync(int userId, List<int> commentIds);
        Task<Like> GetLikesCommentByUser(int commentId, int userId);
        Task<int> GetLikeCountFromPostAsync(int postId);
        Task<int> GetLikeCountFromCommentAsync(int commentId);

    }
}
