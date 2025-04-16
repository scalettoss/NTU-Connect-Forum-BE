using ForumBE.DTOs.Roles;
using ForumBE.Models;

namespace ForumBE.Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetRoleByName(string roleName);
    }
}
