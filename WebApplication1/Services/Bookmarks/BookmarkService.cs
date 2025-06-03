using AutoMapper;
using ForumBE.DTOs.Bookmarks;
using ForumBE.DTOs.Exception;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Bookmarks;
using ForumBE.Repositories.Posts;
using ForumBE.Services.ActivitiesLog;

namespace ForumBE.Services.Bookmarks
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IPostRepository _postRepository;
        private readonly ClaimContext _userContextService;
        private readonly IMapper _mapper;
        private readonly IActivityLogService _activityLogService;

        public BookmarkService(
            IBookmarkRepository bookmarkRepository, 
            ClaimContext userContextService, 
            IPostRepository postRepository, 
            IMapper mapper,
            IActivityLogService activityLogService)
        {
            _bookmarkRepository = bookmarkRepository;
            _postRepository = postRepository;
            _userContextService = userContextService;
            _mapper = mapper;
            _activityLogService = activityLogService;
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

        public async Task<IEnumerable<BookmarkResponseDto>> GetAllBookmarksByUserAsync(int userId)
        {
            var bookmarks = await _bookmarkRepository.GetAllByUserAsync(userId);
            var bookmarkMap = _mapper.Map<IEnumerable<BookmarkResponseDto>>(bookmarks);
            return bookmarkMap;
        }

        public async Task<bool> ToggleBookmarkAsync(BookmarkCreateRequestDto input)
        {
            var userId = _userContextService.GetUserId();
            var existingBookmark = await _bookmarkRepository.GetByPostAsync(input.PostId);
            if (existingBookmark != null && existingBookmark.UserId == userId)
            {
                if (existingBookmark.IsDeleted)
                {
                    existingBookmark.IsDeleted = false;
                    await _bookmarkRepository.UpdateAsync(existingBookmark);
                    return true;
                }
                else
                {
                    existingBookmark.IsDeleted = true;
                    await _bookmarkRepository.UpdateAsync(existingBookmark);
                    return true;
                }
            }

            var existingPost = await _postRepository.GetByIdAsync(input.PostId);
            if (existingPost == null)
            {
                throw new HandleException("Post not found", 404);
            }

            var data = new Bookmark
            {
                PostId = input.PostId,
                UserId = userId,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };
            await _bookmarkRepository.AddAsync(data);
            return true;
        }
    }
}
