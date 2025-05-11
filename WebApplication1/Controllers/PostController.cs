using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ResponseBase> GetAllPost([FromQuery] PaginationDto input)
        {
            var posts = await _postService.GetAllPostsAsync(input);
            return ResponseBase.Success(posts);
        }

        [AllowAnonymous]
        [HttpGet("get-by-category/{slug}")]
        public async Task<ResponseBase> GetAllPostByCategory(string slug, [FromQuery] PaginationDto input)
        {
            var posts = await _postService.GetAllPostByCategoryAsync(input, slug);
            return ResponseBase.Success(posts);
        }

        [AllowAnonymous]
        [HttpGet("slug/{slug}")]
        public async Task<ResponseBase> GetPostBySlug(string slug)
        {
            var category = await _postService.GetPostBySlugAsync(slug);
            return ResponseBase.Success(category);
        }


        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetPostById(int id)
        {
            var category = await _postService.GetPostByIdAsync(id);
            return ResponseBase.Success(category);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPost]
        public async Task<ResponseBase> CreatePost([FromBody] PostCreateRequestDto input)
        {
            var createdId = await _postService.CreatePostAsync(input);
            return ResponseBase.Success(createdId);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdatePost(int id, [FromBody] PostUpdateRequest input)
        {
            var isUpdated = await _postService.UpdatePostAsync(id, input);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Update post failed.");
            }
            return ResponseBase.Success("Update post successfully");
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeletePost(int id)
        {
            var isDeleted = await _postService.DeletePostAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete post failed.");
            }
            return ResponseBase.Success("Delete post successfully");
        }

        [AllowAnonymous]
        [HttpGet("latest-posts")]
        public async Task<ResponseBase> GetLatestPostsByCategory([FromQuery] string? sortBy)
        {
            var posts = await _postService.GetLatestPostsAsync(sortBy);
            return ResponseBase.Success(posts);
        }
    }
}
