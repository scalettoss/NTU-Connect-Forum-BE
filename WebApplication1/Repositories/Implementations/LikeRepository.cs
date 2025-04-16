using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Implementations
{
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        public LikeRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<int> GetLikeCountFromCommentAsync(int commentId)
        {
            var count = await _context.Likes
                .Where(like => like.CommentId == commentId && like.IsLike == true)
                .CountAsync();

            return count;
        }
        public async Task<int> GetLikeCountFromPostAsync(int postId)
        {
            var count = await _context.Likes
                .Where(like => like.PostId == postId && like.IsLike == true)
                .CountAsync();

            return count;
        }
        public async Task<Like> GetLikesCommentByUser(int commentId, int userId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(like => like.CommentId == commentId && like.UserId == userId);

            return like;
        }
        public async Task<Like> GetLikesPostByUser(int postId, int userId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(like => like.PostId == postId && like.UserId == userId);
            
            return like;
        }
    }
}
