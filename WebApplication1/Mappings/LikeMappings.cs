using AutoMapper;
using ForumBE.DTOs.Likes;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class LikeMappings : Profile
    {
        public LikeMappings()
        {
            CreateMap<Like, LikeResponseDto>();
            CreateMap<LikeToggleRequestDto, Like>();
        }
    }
}
