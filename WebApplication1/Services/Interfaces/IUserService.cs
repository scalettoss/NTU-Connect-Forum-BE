using ForumBE.DTOs.Users;
using ForumBE.Models;

namespace ForumBE.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserResponseDto>> GetAllUserAsync();
        Task<GetUserResponseDto> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        Task AddUserAsync(User user);
        Task<GetUserResponseDto> GetUserByEmailAsync(string email);
    }
}
