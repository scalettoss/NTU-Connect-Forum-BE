using ForumBE.Models;
using ForumBE.Repositories.Interfaces;

namespace ForumBE.Repositories.Implementations
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
