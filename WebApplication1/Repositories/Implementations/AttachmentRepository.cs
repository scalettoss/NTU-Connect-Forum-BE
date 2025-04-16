using ForumBE.Models;
using ForumBE.Repositories.Interfaces;

namespace ForumBE.Repositories.Implementations
{
    public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
