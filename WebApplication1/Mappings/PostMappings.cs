using AutoMapper;
using ForumBE.DTOs.Posts;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class PostMappings : Profile
    {
        public PostMappings()
        {
            CreateMap<Post, PostResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategorySlug, opt => opt.MapFrom(src => src.Category.Slug))
                .ForMember(dest => dest.PostSlug, opt => opt.MapFrom(src => src.Slug))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.UserProfile.AvatarUrl))
                .ForMember(dest => dest.FileTypes, opt => opt.MapFrom(src => src.Attachments.Select(a => a.FileType).ToList()))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count()))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count()))
                .ForMember(dest => dest.FileUrls, opt => opt.MapFrom(src => src.Attachments.Select(a => a.FileUrl).ToList()))
                ;


            CreateMap<PostCreateRequestDto, Post>();
            CreateMap<PostUpdateRequest, Post>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
