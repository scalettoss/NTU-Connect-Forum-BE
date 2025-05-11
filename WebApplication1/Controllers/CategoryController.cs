using ForumBE.DTOs.Categories;
using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ResponseBase> GetAllCategory([FromQuery] PaginationDto request)
        {
            var catogories = await _categoryService.GetAllCategoriesAsync(request);
            return ResponseBase.Success(catogories);
        }

        [AllowAnonymous]
        [HttpGet("slug/{slug}")]
        public async Task<ResponseBase> GetCategoryById(string slug)
        {
            var category = await _categoryService.GetCategoryBySlugAsync(slug);
            return ResponseBase.Success(category);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return ResponseBase.Success(category);
        }

        [AuthorizeRoles(ConstantString.Moderator)]
        [HttpPost]
        public async Task<ResponseBase> CreateCategory([FromBody] CategoryCreateRequestDto input)
        {
            var isCreated = await _categoryService.CreateCategoryAsync(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Created category failed.");
            }
            return ResponseBase.Success("Created category successfully.");
        }

        [AuthorizeRoles(ConstantString.Moderator)]
        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdateCategory(int id, [FromBody] CategoryUpdateRequestDto input)
        {
            var isUpdated = await _categoryService.UpdateCategoryAsync(id, input);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Update category failed.");
            }
            return ResponseBase.Success("Update category successfully");
        }

        [AuthorizeRoles(ConstantString.Moderator)]
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteCategory(int id)
        {
            var isDeleted = await _categoryService.DeleteCategoryAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete category failed.");
            }
            return ResponseBase.Success("Delete category successfully");
        }

    }
}
