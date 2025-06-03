using ForumBE.DTOs.Bookmarks;
using ForumBE.Models;

namespace ForumBE.Services.Bookmarks
{
    public interface IBookmarkService
    {
        Task<IEnumerable<BookmarkResponseDto>> GetAllBookmarksAsync();
        Task<IEnumerable<BookmarkResponseDto>> GetAllBookmarksByUserAsync(int userId);
        Task<BookmarkResponseDto> GetBookmarkByIdAsync(int id);
        Task<bool> ToggleBookmarkAsync(BookmarkCreateRequestDto input);
    }
}
