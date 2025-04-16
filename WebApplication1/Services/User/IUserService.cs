using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Users;


namespace ForumBE.Services.User
{
    public interface IUserService
    {
        Task<PaginationData<UserResponseDto>> GetAllUserAsync(PaginationParams input);
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
        Task<UserResponseDto> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(int id, UserUpdateProfilesRequestDto input);
        Task<bool> ChangePasswordAsync(UserChangePasswordRequestDto input);
        Task<bool> ActiveUser(int id, bool isActive);
    }
}
