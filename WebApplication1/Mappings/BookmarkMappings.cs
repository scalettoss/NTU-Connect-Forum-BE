using AutoMapper;
using ForumBE.DTOs.Bookmarks;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class BookmarkMappings : Profile
    {
       public BookmarkMappings()
        {
            CreateMap<Bookmark, BookmarkResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Post.Category.Name))
                .ForMember(dest => dest.CategorySlug, opt => opt.MapFrom(src => src.Post.Category.Slug))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(src => src.Post.Title))
                .ForMember(dest => dest.PostContent, opt => opt.MapFrom(src => src.Post.Content))
                .ForMember(dest => dest.PostSlug, opt => opt.MapFrom(src => src.Post.Slug))
                ;

            CreateMap<BookmarkCreateRequestDto, Bookmark>();
        }
    }
}
