using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Implementations
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
