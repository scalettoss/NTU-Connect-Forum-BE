using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.UserProflies;
using ForumBE.DTOs.Users;


namespace ForumBE.Services.User
{
    public interface IUserService
    {
        Task<PagedList<UserResponseDto>> GetAllUserAsync(PaginationDto input);
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
        Task<UserResponseDto> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(int id, UserUpdateProfilesRequestDto input);
        Task<bool> ChangePasswordAsync(UserChangePasswordRequestDto input);
        Task<bool> ActiveUser(int id, bool isActive);
        Task<UserProfileResponseDto> GetUserInformationAsync(int id);
        Task<bool> UpdateUserInformationAsync(UserProfileUpdateRequestDto input, int id);
    }
}
