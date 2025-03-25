using ForumBE.DTOs.Users;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            var list = await _context.Users
                .Include(u => u.UserProfiles).ToListAsync();
            return list;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserProfiles)
                .FirstOrDefaultAsync(u => u.UserId == id);
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(u => u.UserProfiles)
                .Include(u => u.Role)  
                .FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
