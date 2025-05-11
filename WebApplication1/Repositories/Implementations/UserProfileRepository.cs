using ForumBE.Models;
using ForumBE.Repositories.Generics;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Implementations
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task<UserProfile> GetByIdAsync(int id)
        {
            return await _context.UserProfiles
                .Include(x => x.User)
                .Include(x => x.User.Posts)
                .Include(x => x.User.Comments)
                .Include(x => x.User.Likes)
                .Include(x => x.User.Role)
                .Where(x => x.UserId == id)
                .FirstOrDefaultAsync(x => x.UserId == id);
        }
    }
}
