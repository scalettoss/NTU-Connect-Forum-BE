using ForumBE.DTOs.Bookmarks;
using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Bookmarks
{
    public interface IBookmarkRepository : IGenericRepository<Bookmark>
    {
        Task<Bookmark> IsExistingBookmark(int userId, int postId);
    }
}
