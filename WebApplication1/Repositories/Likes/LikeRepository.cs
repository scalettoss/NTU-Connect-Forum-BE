using ForumBE.Models;
using ForumBE.Repositories.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ForumBE.Repositories.Likes
{
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        public LikeRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<int> GetLikeCountFromCommentAsync(int commentId)
        {
            var count = await _context.Likes
                .Where(like => like.CommentId == commentId)
                .CountAsync();

            return count;
        }
        public async Task<int> GetLikeCountFromPostAsync(int postId)
        {
            var count = await _context.Likes
                .Where(like => like.PostId == postId)
                .CountAsync();

            return count;
        }

        public async Task<List<int>> GetLikedCommentIdsByUserAsync(int userId, List<int> commentIds)
        {
            return await _context.Likes
                .Where(l => l.UserId == userId && l.CommentId != null && commentIds.Contains(l.CommentId.Value))
                .Select(l => l.CommentId.Value)
                .ToListAsync();
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
