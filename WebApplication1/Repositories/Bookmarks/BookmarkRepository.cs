using ForumBE.DTOs.Bookmarks;
using ForumBE.Models;
using ForumBE.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Bookmarks
{
    public class BookmarkRepository : GenericRepository<Bookmark>, IBookmarkRepository
    {
        public BookmarkRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Bookmark> IsExistingBookmark(int userId, int postId)
        {
            var existingBookmark = await _context.Bookmarks.FirstOrDefaultAsync(x => x.UserId == userId && x.PostId == postId);
            return existingBookmark;
        }

        public async Task<bool> IsAlreadyBookmark(int postId ,int userId)
        {
            var bookmark = await _context.Bookmarks
                                 .AnyAsync(b => b.UserId == userId && b.PostId == postId && b.IsDeleted == false);

            return bookmark;

        }

        public async Task<IEnumerable<Bookmark>> GetAllByUserAsync(int userId)
        {
            var bookmarks = await _context.Bookmarks
                .Include(b => b.User)
                .Include(b => b.Post)
                .Include(b => b.Post.Category)
                .Where(b => b.UserId == userId && b.IsDeleted == false)
                .ToListAsync();

            return bookmarks;
        }

        public async Task<Bookmark> GetByPostAsync(int postId)
        {
            var bookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.PostId == postId);

            return bookmark;
        }
    }
}
