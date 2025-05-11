using ForumBE.DTOs.Likes;
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
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [AuthorizeRoles(ConstantString.Moderator)]
        [HttpGet]
        public async Task<ResponseBase> GetAllLikes()
        {
            var likes = await _likeService.GetAllLikesAsync();
            return ResponseBase.Success(likes);
        }

        [AuthorizeRoles(ConstantString.Moderator)]
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetLikeById(int id)
        {
            var like = await _likeService.GetLikeByIdAsync(id);
            return ResponseBase.Success(like);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpGet("count-post/{postId}")]
        public async Task<ResponseBase> GetLikeCountFromPost(int postId)
        {
            var count = await _likeService.GetLikeCountFromPostAsync(postId);
            return ResponseBase.Success(count);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpGet("count-comment/{commentId}")]
        public async Task<ResponseBase> GetLikeCountFromComment(int commentId)
        {
            var count = await _likeService.GetLikeCountFromCommentAsync(commentId);
            return ResponseBase.Success(count);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPost("toggle")]
        public async Task<ResponseBase> ToggleLike([FromBody] LikeToggleRequestDto input)
        {
            var isToggled = await _likeService.ToggleLikeAsync(input);
            if (!isToggled)
            {
                return ResponseBase.Fail("Toggle like failed.");
            }
            return ResponseBase.Success("Toggle like successfully");
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteLike(int id)
        {
            var isDeleted = await _likeService.DeleteLikeAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete like failed.");
            }
            return ResponseBase.Success("Delete like successfully");
        }
    }
}
