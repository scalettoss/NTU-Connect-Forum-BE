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
    }
}
