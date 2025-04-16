using AutoMapper;
using ForumBE.DTOs.Roles;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class RoleMappings : Profile
    {
        public RoleMappings()
        {
            CreateMap<Role, RoleResponseDto>();
            CreateMap<RoleCreateRequestDto, Role>();
            CreateMap<RoleUpdateRequestDto, Role>();
        }
    }
}
