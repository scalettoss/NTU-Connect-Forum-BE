using ForumBE.DTOs.Paginations;
using ForumBE.Models;
using ForumBE.Repositories.Generics;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Users
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

        public async Task<User> GetByUserIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            return user;
        }

        public override async Task<PagedList<User>> GetAllPagesAsync(PaginationDto input)
        {
            var query = _context.Users
                .Include(u => u.UserProfile)
                .Include(u => u.Role)
                .Where(u => u.IsDeleted == false)
                .AsQueryable();

            return await PagedList<User>.CreateAsync(query, input.PageNumber, input.PageSize);
        }
    }
}
