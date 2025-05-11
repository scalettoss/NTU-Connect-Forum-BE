using ForumBE.DTOs.Paginations;
using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUserIdAsync(int userId);
        Task<User> GetByEmailAsync(string email);
        Task ChangePasswordAsync(User user);
    }

}
