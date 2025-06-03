using ForumBE.DTOs.Bookmarks;
using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.Bookmarks;
using ForumBE.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [AuthorizeRoles(ConstantString.Admin)]
        [HttpGet]
        public async Task<ResponseBase> GetAllBookmarks()
        {
            var bookmarks = await _bookmarkService.GetAllBookmarksAsync();
            return ResponseBase.Success(bookmarks);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpGet("user/{id}")]
        public async Task<ResponseBase> GetAllBookmarkByUserId(int id)
        {
            var bookmark = await _bookmarkService.GetAllBookmarksByUserAsync(id);
            return ResponseBase.Success(bookmark);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPost("toggle")]
        public async Task<ResponseBase> ToggleBookmark([FromBody] BookmarkCreateRequestDto request)
        {
            var isToggled = await _bookmarkService.ToggleBookmarkAsync(request);
            if (!isToggled)
            {
                return ResponseBase.Fail("Toggle bookmark failed");
            }
            return ResponseBase.Success("Toggle bookmark success");
        }
    }
}
