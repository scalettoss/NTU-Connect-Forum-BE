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

            CreateMap<Post, PostAdminResponseDto>();
            //    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            //    .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.UserProfile.AvatarUrl))
            //    .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count()))
            //    .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count()))
            //    .ForMember(dest => dest.ConfidenceScore, opt => opt.MapFrom(src => src.ScamDetection.ConfidenceScore))
            //    .ForMember(dest => dest.IsScam, opt => opt.MapFrom(src => src.ScamDetection.ModelPrediction))
            //    .ForMember(dest => dest.FileTypes, opt => opt.MapFrom(src => src.Attachments.Select(a => a.FileType).ToList()))
            //    .ForMember(dest => dest.FileUrls, opt => opt.MapFrom(src => src.Attachments.Select(a => a.FileUrl).ToList()))
            //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<PostCreateRequestDto, Post>();
            CreateMap<PostUpdateRequestDto, Post>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
