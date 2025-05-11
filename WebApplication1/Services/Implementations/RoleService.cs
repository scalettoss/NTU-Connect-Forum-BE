using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Roles;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Interfaces;

namespace ForumBE.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            var rolesMap = _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
            return rolesMap;
        }

        public async Task<RoleResponseDto> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new HandleException("Role not found", 404);
            }
            var roleMap = _mapper.Map<RoleResponseDto>(role);
            return roleMap;
        }

        public async Task<bool> CreateRoleAsync(RoleCreateRequestDto input)
        {
            try
            {
                var existingRole = await _roleRepository.GetRoleByName(input.RoleName);
                if (existingRole != null)
                {
                    throw new HandleException("Role has existing", 400);
                }
                var roleEntity = _mapper.Map<Role>(input);
                roleEntity.CreatedAt = DateTime.Now;
                await _roleRepository.AddAsync(roleEntity);

                return true;
            }
            catch
            {
                throw;
            }

        }
        public async Task<bool> UpdateRoleAsync(int roleId, RoleUpdateRequestDto input)
        {
            try
            {
                var existingRole = _roleRepository.GetByIdAsync(roleId);
                if (existingRole == null)
                {
                    throw new HandleException("Role not found", 404);
                }
                var roleEntity = _mapper.Map<Role>(input);
                roleEntity.UpdatedAt = DateTime.Now;
                await _roleRepository.UpdateAsync(roleEntity);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            try
            {
                var existingRole = await _roleRepository.GetByIdAsync(roleId);
                if (existingRole == null)
                {
                    throw new HandleException("Role not found", 404);
                }
                await _roleRepository.DeleteAsync(existingRole);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
