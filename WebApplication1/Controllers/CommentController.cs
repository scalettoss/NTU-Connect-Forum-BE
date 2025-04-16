using ForumBE.DTOs.Comments;
using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.Implementations;
using ForumBE.Services.Interfaces;
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

        [AuthorizeRoles(ConstantsString.User)]
        [HttpGet]
        public async Task<ResponseBase> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return ResponseBase.Success(comments);
        }

        [AuthorizeRoles(ConstantsString.User)]
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetCommentById(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            return ResponseBase.Success(comment);
        }

        [AuthorizeRoles(ConstantsString.User)]
        [HttpPost]
        public async Task<ResponseBase> CreateComment([FromBody] CommentCreateRequestDto input)
        {
            var isCreated = await _commentService.CreateCommentAsync(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Created comment failed.");
            }
            return ResponseBase.Success("Created comment successfully");
        }

        [AuthorizeRoles(ConstantsString.User)]
        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdateComment(int id, [FromBody] CommentUpdateRequestDto input)
        {
            var isUpdated = await _commentService.UpdateCommentAsync(id, input);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Update comment failed.");
            }
            return ResponseBase.Success("Update comment successfully");
        }

        [AuthorizeRoles(ConstantsString.User)]
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteComment(int id)
        {
            var isDeleted = await _commentService.DeleteCommentAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete comment failed.");
            }
            return ResponseBase.Success("Delete comment successfully");
        }
    }
}
