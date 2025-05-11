using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Posts
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> CountByCategoryAsync(int categoryId)
        {
            return await _context.Posts
                    .Where(p => p.CategoryId == categoryId && p.IsDeleted == false)
                    .CountAsync();
        }

        public Task<int> CountPostCommentAsync(int postId)
        {
            return _context.Comments
                .Where(c => c.PostId == postId && c.IsDeleted == false)
                .CountAsync();
        }

        public Task<int> CountPostLikeAsync(int postId)
        {
            return _context.Likes
                .Where(l => l.PostId == postId && l.IsDeleted == false)
                .CountAsync();
        }

        public override async Task<PagedList<Post>> GetAllPagesAsync(PaginationDto input)
        {
            var query = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Attachments)
                .Include(p => p.User.UserProfile)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Where(p => p.Status == ConstantString.Approved && p.IsDeleted == false)
                .AsQueryable();

            switch (input.SortBy?.ToLower())
            {
                case "oldest":
                    query = query.OrderBy(p => p.CreatedAt);
                    break;

                case "popular":
                    query = query.OrderByDescending(p => p.Comments.Count * 2 + p.Likes.Count * 1.5);
                    break;

                case "newest":
                default:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            return await PagedList<Post>.CreateAsync(query, input.PageNumber, input.PageSize);
        }

        public async Task<PagedList<Post>> GetAllPagesByCategoryAsync(int categoryId, PaginationDto input)
        {
            var query = _context.Posts
                .Where(p => p.CategoryId == categoryId && p.Status == ConstantString.Approved && p.IsDeleted == false)
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(p => p.Attachments)
                .Include(p => p.User.UserProfile)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .AsQueryable();

            switch (input.SortBy?.ToLower())
            {
                case "newest":
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
                case "oldest":
                    query = query.OrderBy(p => p.CreatedAt);
                    break;
                case "popular":
                    query = query.OrderByDescending(p => p.Comments.Count * 2 + p.Likes.Count * 1.5);
                    break;
                default:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }
            return await PagedList<Post>.CreateAsync(query, input.PageNumber, input.PageSize);
        }

        public async Task<IEnumerable<Post>> GetLatestPostsAsync(string sortBy)
        {
            var query = _context.Posts
                .Where(p => p.Status == ConstantString.Approved && p.IsDeleted == false)
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(p => p.Attachments)
                .Include(p => p.User.UserProfile)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .AsNoTracking();

            switch (sortBy?.ToLower())
            {
                case "newest":
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
                case "oldest":
                    query = query.OrderBy(p => p.CreatedAt);
                    break;
                case "popular":
                    query = query.OrderByDescending(p => p.Comments.Count * 2 + p.Likes.Count * 1.5);
                    break;
                default:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            var latestPosts = await query.Take(3).ToListAsync();

            return latestPosts;
        }

        public override async Task<Post> GetByIdAsync(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Attachments)
                .Include(p => p.User.UserProfile)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.PostId == id && p.Status == ConstantString.Approved && p.IsDeleted == false);
            return post;
        }

        public async Task<bool> HasUserLikedPostAsync(int postId, int userId)
        {
            return await _context.Likes
                .AnyAsync(like => like.PostId == postId && like.UserId == userId);
        }

        public async Task<Post> GetPostBySlugAsync(string slug)
        {
            var post = await _context.Posts
               .Include(p => p.User)
               .Include(p => p.Attachments)
               .Include(p => p.User.UserProfile)
               .Include(p => p.Comments)
               .Include(p => p.Likes)
               .Include(p => p.Category)
               .FirstOrDefaultAsync(p => p.Slug == slug && p.Status == ConstantString.Approved && p.IsDeleted == false);
            return post;
        }
    }
}
