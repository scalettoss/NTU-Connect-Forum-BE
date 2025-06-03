using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Attachments
{
    public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
