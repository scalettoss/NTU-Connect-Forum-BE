using ForumBE.DTOs.Categories;
using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [AuthorizeRoles(ConstantsString.User)]
        [HttpGet]
        public async Task<ResponseBase> GetAllPost([FromQuery] PaginationParams input)
        {
            var posts = await _postService.GetAllPostsAsync(input);
            return ResponseBase.Success(posts.Data, posts.Pagination);
        }

        [AuthorizeRoles(ConstantsString.User)]
        [HttpGet("get-by-category/{categoryId}")]
        public async Task<ResponseBase> GetAllPostByCategoryId(int categoryId, [FromQuery] PaginationParams input)
        {
            var posts = await _postService.GetAllPostByCategoryIdAsync(categoryId, input);
            return ResponseBase.Success(posts.Data, posts.Pagination);
        }

        [AuthorizeRoles(ConstantsString.User)]
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetPostById(int id)
        {
            var category = await _postService.GetPostByIdAsync(id);
            return ResponseBase.Success(category);
        }

        [AuthorizeRoles(ConstantsString.User)]
        [HttpPost]
        public async Task<ResponseBase> CreatePost([FromBody] PostCreateRequestDto input)
        {
            var isCreated = await _postService.CreatePostAsync(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Created post failed.");
            }
            return ResponseBase.Success("Created post successfully");
        }

        [AuthorizeRoles(ConstantsString.User)]
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

        [AuthorizeRoles(ConstantsString.User)]
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

        
    }
}
