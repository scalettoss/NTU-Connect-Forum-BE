using AutoMapper;
using ForumBE.DTOs.Comments;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class CommentMappings : Profile
    {
        public CommentMappings()
        {
            CreateMap<Comment, CommentResponseDto>();
            CreateMap<CommentCreateRequestDto, Comment>();
            CreateMap<CommentUpdateRequestDto, Comment>();
        }
    }
}
