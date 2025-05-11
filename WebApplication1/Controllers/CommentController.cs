using ForumBE.DTOs.Comments;
using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.Comments;
using ForumBE.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [AuthorizeRoles(ConstantString.Admin)]
        [HttpGet]
        public async Task<ResponseBase> GetAllComments([FromQuery] PaginationDto request)
        {
            var comments = await _commentService.GetAllCommentsAsync(request);
            return ResponseBase.Success(comments);
        }

        [AllowAnonymous]
        [HttpGet("by-post/{id}")]
        public async Task<ResponseBase> GetAllCommentsByPost([FromQuery] PaginationDto request, int id)
        {
            var comments = await _commentService.GetAllCommentsByPostAsync(request, id);
            return ResponseBase.Success(comments);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetCommentById(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            return ResponseBase.Success(comment);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPost]
        public async Task<ResponseBase> CreateComment([FromBody] CommentCreateRequestDto request)
        {
            var comments = await _commentService.CreateCommentAsync(request);
            return ResponseBase.Success(comments);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdateComment(int id, [FromBody] CommentUpdateRequestDto request)
        {
            var isUpdated = await _commentService.UpdateCommentAsync(request, id);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Update comment failed.");
            }
            return ResponseBase.Success("Update comment successfully");
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpDelete()]
        public async Task<ResponseBase> DeleteComment([FromQuery] int id, [FromQuery] int userId)
        {
            var isDeleted = await _commentService.DeleteCommentAsync(id, userId);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete comment failed.");
            }
            return ResponseBase.Success("Delete comment successfully");
        }
    }
}
