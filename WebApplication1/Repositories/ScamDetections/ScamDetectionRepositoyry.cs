using ForumBE.Models;
using ForumBE.Repositories.Generics;
using ForumBE.Repositories.Reports;

namespace ForumBE.Repositories.ScamDetections
{
    public class ScamDetectionRepositoyry : GenericRepository<ScamDetection>, IScamDetectionRepositoyry
    {
        public ScamDetectionRepositoyry(ApplicationDbContext context) : base(context)
        {
        }
    }
}
