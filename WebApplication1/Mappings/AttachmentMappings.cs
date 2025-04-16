using AutoMapper;
using ForumBE.DTOs.Attachments;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class AttachmentMappings : Profile
    {
        public AttachmentMappings()
        {
            CreateMap<Attachment, AttachmentResponseDto>();
            CreateMap<AttachmentCreateRequestDto, Attachment>();
            CreateMap<AttachmentUpdateRequestDto, Attachment>();
        }
    }
}
