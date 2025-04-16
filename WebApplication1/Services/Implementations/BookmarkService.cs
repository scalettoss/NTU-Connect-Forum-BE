using AutoMapper;
using ForumBE.DTOs.Bookmarks;
using ForumBE.DTOs.Exception;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Interfaces;

namespace ForumBE.Services.Implementations
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IPostRepository _postRepository;
        private readonly ClaimContext _userContextService;
        private readonly IMapper _mapper;
        public BookmarkService(IBookmarkRepository bookmarkRepository, ClaimContext userContextService, IPostRepository postRepository, IMapper mapper)
        {
            _bookmarkRepository = bookmarkRepository;
            _postRepository = postRepository;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookmarkResponseDto>> GetAllBookmarksAsync()
        {
            var bookmarks = await _bookmarkRepository.GetAllAsync();
            var bookmarkMap = _mapper.Map<IEnumerable<BookmarkResponseDto>>(bookmarks);
            return bookmarkMap;
        }

        public async Task<BookmarkResponseDto> GetBookmarkByIdAsync(int id)
        {
            var bookmark = await _bookmarkRepository.GetByIdAsync(id);
            if (bookmark == null)
            {
                throw new HandleException("Bookmark not found", 404);
            }
            var bookmarkMap = _mapper.Map<BookmarkResponseDto>(bookmark);
            return bookmarkMap;
        }

        public async Task<bool> CreateBookmarkAsync(BookmarkCreateRequestDto input)
        {
            try
            {
                var userId = _userContextService.GetUserId();
                var existingPost = await _postRepository.GetByIdAsync(input.PostId);

                if (existingPost == null)
                {
                    throw new HandleException("Post not found", 404);
                }

                var existingBookmark = await _bookmarkRepository.IsExistingBookmark(userId, input.PostId);

                if (existingBookmark != null)
                {
                    throw new HandleException("Bookmark already exists", 400);
                }

                if (existingPost.UserId == userId)
                {
                    throw new HandleException("You cannot bookmark your own post", 400);
                }
                var bookmark = _mapper.Map<Bookmark>(input);
                bookmark.UserId = userId;
                bookmark.CreatedAt = DateTime.UtcNow;
                await _bookmarkRepository.AddAsync(bookmark);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteBookmarkAsync(int id)
        {
            try
            {
                var userId = _userContextService.GetUserId();

                var bookmark = await _bookmarkRepository.GetByIdAsync(id);

                if (bookmark == null)
                {
                    throw new HandleException("Bookmark not found", 404);
                }

                if (bookmark.UserId != userId)
                {
                    throw new HandleException("You cannot delete this bookmark", 403);
                }

                await _bookmarkRepository.DeleteAsync(bookmark);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
