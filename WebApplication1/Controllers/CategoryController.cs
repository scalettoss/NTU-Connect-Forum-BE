using ForumBE.DTOs.Categories;
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

        [AuthorizeRoles(ConstantsString.User)]
        [HttpGet]
        public async Task<ResponseBase> GetAllCategory()
        {
            var catogories = await _categoryService.GetAllCategoriesAsync();
            return ResponseBase.Success(catogories);
        }

        [AuthorizeRoles(ConstantsString.User)]
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return ResponseBase.Success(category);
        }

        [AuthorizeRoles(ConstantsString.Moderator)]
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

        [AuthorizeRoles(ConstantsString.Moderator)]
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

        [AuthorizeRoles(ConstantsString.Moderator)]
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
