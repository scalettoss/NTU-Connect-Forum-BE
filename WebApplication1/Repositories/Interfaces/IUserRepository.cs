using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Models;

namespace ForumBE.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<PagedResult<User>> GetAllUserProfileAsync(PaginationParams input);
        Task<User> GetProfileByIdAsync(int userId);
        Task<User> GetByEmailAsync(string email);
        Task ChangePasswordAsync(User user);
    }

}
