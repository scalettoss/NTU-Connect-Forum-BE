using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.UserProflies;
using ForumBE.DTOs.Users;
using ForumBE.Helpers;
using ForumBE.Response;
using ForumBE.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ResponseBase> GetAllUsers([FromQuery] PaginationDto request)
        {
            var users = await _userService.GetAllUserAsync(request);
            return ResponseBase.Success(users);
        }

        //[AuthorizeRoles(ConstantsString.Admin)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ResponseBase> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return ResponseBase.Success(user);
        }

        [AllowAnonymous]
        [HttpGet("information/{id}")]
        public async Task<ResponseBase> GetUserInformation(int id)
        {
            var user = await _userService.GetUserInformationAsync(id);
            return ResponseBase.Success(user);
        }


        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<ResponseBase> SearchUser([FromBody] UserSearchRequestDto request)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
            return ResponseBase.Success(user);
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPut("{id}")]
        public async Task<ResponseBase> UpdateUserInformation(int id, [FromForm] UserProfileUpdateRequestDto input)
        {
            var isUpdated = await _userService.UpdateUserInformationAsync(input, id);
            if (!isUpdated)
            {
                return ResponseBase.Fail("Update user failed.");
            }
            return ResponseBase.Success("Update user successfully");
        }

        [AuthorizeRoles(ConstantString.Admin)]
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteUser(int id)
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete user failed.");
            }
            return ResponseBase.Success("Delete user successfully");
        }

        [AuthorizeRoles(ConstantString.User)]
        [HttpPost("change-password")]
        public async Task<ResponseBase> ChangePassword([FromBody] UserChangePasswordRequestDto request)
        {
            var isChanged = await _userService.ChangePasswordAsync(request);

            if (!isChanged)
            {
                return ResponseBase.Fail("Change password failed.");
            }
            return ResponseBase.Success("Change password successfully.");
        }

        [AuthorizeRoles(ConstantString.Moderator)]
        [HttpPost("change-active")]
        public async Task<ResponseBase> ActiveUser(UserActiveRequestDto request)
        {
            var isComplete = await _userService.ActiveUser(request.UserId, request.IsActive);

            if (!isComplete)
            {
                return ResponseBase.Fail("Change actived failed.");
            }
            return ResponseBase.Success("Change actived successfully.");
        }

        
    }
}
