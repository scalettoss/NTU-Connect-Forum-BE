using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Users;
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

        public async Task<PagedList<User>> GetByCondition(PaginationDto input, AdvancedUserSearchRequestDto condition)
        {
            var query = _context.Users
                .Include(u => u.UserProfile)
                .Include(u => u.Role)
                .Where(u => u.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(condition.Name))
            {
                var keyword = condition.Name.ToLower();
                query = query.Where(u =>
                    (u.FirstName + " " + u.LastName).ToLower().Contains(keyword) ||
                    (u.LastName + " " + u.FirstName).ToLower().Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(condition.Email))
            {
                query = query.Where(u => u.Email.Contains(condition.Email));
            }

            if (condition.RoleId.HasValue)
            {
                query = query.Where(u => u.RoleId == condition.RoleId);
            }

            if (condition.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == condition.IsActive.Value);
            }

            return await PagedList<User>.CreateAsync(query, input.PageNumber, input.PageSize);

        }
    }
}
