using ForumBE.DTOs.Bookmarks;
using ForumBE.Models;

namespace ForumBE.Repositories.Interfaces
{
    public interface IBookmarkRepository : IGenericRepository<Bookmark>
    {
        Task<Bookmark> IsExistingBookmark(int userId, int postId);
    }
}
