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

        public async Task<PagedList<Comment>> GetAllCommentsByPost(PaginationDto input, int postId)
        {
            var query = _context.Comments
                .Include(c => c.User)
                .Include(c => c.User.UserProfile)
                .Include(c => c.Attachments)
                .Include(c => c.Likes)
                .Include(c => c.Post)
                .Where(c => c.PostId == postId)
                .AsNoTracking()
                .AsQueryable();


            switch (input.SortBy?.ToLower())
            {
                case "oldest":
                    query = query.OrderBy(p => p.CreatedAt);
                    break;

                case "popular":
                    query = query.OrderByDescending(p => p.Likes.Count());
                    break;

                case "newest":
                default:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }



            return await PagedList<Comment>.CreateAsync(query, input.PageNumber, input.PageSize);

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
