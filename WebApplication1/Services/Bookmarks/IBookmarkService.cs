using ForumBE.DTOs.Bookmarks;

namespace ForumBE.Services.Bookmarks
{
    public interface IBookmarkService
    {
        Task<IEnumerable<BookmarkResponseDto>> GetAllBookmarksAsync();
        Task<BookmarkResponseDto> GetBookmarkByIdAsync(int id);
        Task<bool> CreateBookmarkAsync(BookmarkCreateRequestDto input);
        Task<bool> DeleteBookmarkAsync(int id);
    }
}
