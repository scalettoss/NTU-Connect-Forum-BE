using AutoMapper;
using ForumBE.DTOs.Categories;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<Category, CategoriesResponseDto>();
            CreateMap<CategoryCreateRequestDto, Category>();
            CreateMap<CategoryUpdateRequestDto, Category>();
        }
    }
}
