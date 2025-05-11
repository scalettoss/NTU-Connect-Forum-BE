using AutoMapper;
using ForumBE.DTOs.Comments;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class CommentMappings : Profile
    {
        public CommentMappings()
        {
            CreateMap<Comment, CommentResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.UserProfile.AvatarUrl))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count()))
                .ForMember(dest => dest.FileUrls, opt => opt.MapFrom(src => src.Attachments.Select(a => a.FileUrl).ToList()))
                .ForMember(dest => dest.FileTypes, opt => opt.MapFrom(src => src.Attachments.Select(a => a.FileType).ToList()));
            CreateMap<CommentCreateRequestDto, Comment>();
            CreateMap<CommentUpdateRequestDto, Comment>();
        }
    }
}
