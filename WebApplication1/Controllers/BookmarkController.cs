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
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetBookmarkById(int id)
        {
            var bookmark = await _bookmarkService.GetBookmarkByIdAsync(id);
            return ResponseBase.Success(bookmark);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPost]
        public async Task<ResponseBase> CreateBookmark([FromBody] BookmarkCreateRequestDto input)
        {
            var isCreated = await _bookmarkService.CreateBookmarkAsync(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Create bookmark failed");
            }
            return ResponseBase.Success("Create bookmark success");
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteBookmark(int id)
        {
            var isDeleted = await _bookmarkService.DeleteBookmarkAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete bookmark failed");
            }
            return ResponseBase.Success("Delete bookmark success");
        }
    }
}
