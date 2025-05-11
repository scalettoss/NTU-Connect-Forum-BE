using AutoMapper;
using ForumBE.DTOs.Categories;
using ForumBE.Helpers;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<Category, CategoryResponseDto>()
                .ForMember(dest => dest.CountTotalPost, opt => opt.MapFrom(src => src.Posts.Count(p => p.Status == ConstantString.Approved && p.IsDeleted == false)));
            CreateMap<CategoryCreateRequestDto, Category>();
            CreateMap<CategoryUpdateRequestDto, Category>();
        }
    }
}
