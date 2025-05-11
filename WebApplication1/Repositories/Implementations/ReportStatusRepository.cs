using ForumBE.Models;
using ForumBE.Repositories.Generics;
using ForumBE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.Implementations
{
    public class ReportStatusRepository : GenericRepository<ReportStatus>, IReportStatusRepository
    {
        public ReportStatusRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
