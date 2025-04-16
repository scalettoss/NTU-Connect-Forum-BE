using ForumBE.DTOs.Roles;
using ForumBE.Models;

namespace ForumBE.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
        Task<RoleResponseDto> GetRoleByIdAsync(int id);
        Task<bool> CreateRoleAsync(RoleCreateRequestDto input);
        Task<bool> UpdateRoleAsync(int roleId, RoleUpdateRequestDto input);
        Task<bool> DeleteRoleAsync(int id);
    }
}
