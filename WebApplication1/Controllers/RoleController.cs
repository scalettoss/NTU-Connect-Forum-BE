using ForumBE.DTOs.Roles;
using ForumBE.Response;
using ForumBE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ResponseBase> GetAllRole()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return ResponseBase.Success(roles);
        }

        [HttpGet("{id}")]
        public async Task<ResponseBase> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            return ResponseBase.Success(role);
        }

        [HttpPost]
        public async Task<ResponseBase> CreateRole([FromBody] RoleCreateRequestDto input)
        {
            var isCreated = await _roleService.CreateRoleAsync(input);
            if (!isCreated)
            {
                return ResponseBase.Fail("Created role failed.");
            }
            return ResponseBase.Success("Created role successfully");
        }

        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdateRole(int id, [FromBody] RoleUpdateRequestDto input)
        {
            var isUpdated = await _roleService.UpdateRoleAsync(id, input);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Update role failed.");
            }
            return ResponseBase.Success("Update role successfully");
        }

        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteRole(int id)
        {
            var isDeleted = await _roleService.DeleteRoleAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete role failed.");
            }
            return ResponseBase.Success("Delete role successfully");
        }
    }
}
