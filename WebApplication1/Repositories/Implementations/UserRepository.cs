using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ForumBE.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }
        public async Task ChangePasswordAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetProfileByIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            return user;
        }

        public async Task<PagedResult<User>> GetAllUserProfileAsync(PaginationParams input)
        {
            var query = _context.Users
                .Include(u => u.UserProfile)
                .Include(u => u.Role)
                .AsNoTracking();

            return await query.ToPagedListAsync(input.PageIndex, input.PageSize);
        }

    }
}
