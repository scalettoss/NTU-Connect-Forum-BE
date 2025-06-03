using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
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
                    .Where(p => p.CategoryId == categoryId)
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
                .Where(p => p.Status == ConstantString.Approved )
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
                .Where(p => p.CategoryId == categoryId && p.Status == ConstantString.Approved)
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
                .Where(p => p.Status == ConstantString.Approved)
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
                .FirstOrDefaultAsync(p => p.PostId == id && p.Status == ConstantString.Approved);

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
               .FirstOrDefaultAsync(p => p.Slug == slug && p.Status == ConstantString.Approved);
            return post;
        }

        public Task<int> GetUserAuthorByPostAsync(int postId)
        {
            var post = _context.Posts
                .Where(p => p.PostId == postId && p.IsDeleted == false)
                .Select(p => p.UserId)
                .FirstOrDefaultAsync();

            return post;
        }

        public async Task<bool> IsSlugExistsAsync(string slug)
        {
            return await _context.Posts.AnyAsync(p => p.Slug == slug);
        }

        public async Task<PagedList<PostAdminResponseDto>> GetAllByAdmin(PaginationDto input, PostSearchRequestDto condition)
        {
            var query = _context.Posts
                .AsQueryable();

            // Apply filters
            switch (condition.Status?.ToLower())
            {
                case "approved":
                    query = query.Where(p => p.Status == ConstantString.Approved);
                    break;
                case "pending":
                    query = query.Where(p => p.Status == ConstantString.Pending);
                    break;
                case "rejected":
                    query = query.Where(p => p.Status == ConstantString.Rejected);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(condition?.Title))
            {
                query = query.Where(p => p.Title != null && EF.Functions.Like(p.Title.ToLower(), $"%{condition.Title.ToLower()}%"));
            }

            // Apply sorting
            switch (input.SortBy?.ToLower())
            {
                case "newest":
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
                case "popular":
                    query = query.OrderByDescending(p => p.Comments.Count * 2 + p.Likes.Count * 1.5);
                    break;
                default:
                    query = query.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            // Project to DTO directly in the query
            var projectedQuery = query.Select(p => new PostAdminResponseDto
            {
                PostId = p.PostId,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                Status = p.Status,
                FullName = p.User.FirstName + " " + p.User.LastName,
                AvatarUrl = p.User.UserProfile.AvatarUrl,
                CommentCount = p.Comments.Count(),
                LikeCount = p.Likes.Count(),
                ConfidenceScore = p.ScamDetection.ConfidenceScore,
                IsScam = p.ScamDetection.ModelPrediction,
                FileTypes = p.Attachments.Select(a => a.FileType).ToList(),
                FileUrls = p.Attachments.Select(a => a.FileUrl).ToList(),
                CategoryName = p.Category.Name,
                UpdatedAt = p.UpdatedAt,
            });

            return await PagedList<PostAdminResponseDto>.CreateAsync(projectedQuery, input.PageNumber, input.PageSize);
        }

        public async Task<Post> GetPostByAdminAsync(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Attachments)
                .Include(p => p.User.UserProfile)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Category)
                .Include(p => p.ScamDetection)
                .FirstOrDefaultAsync(p => p.PostId == id);

            return post;
        }
    }
}
