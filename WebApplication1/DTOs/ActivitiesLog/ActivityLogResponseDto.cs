using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.ActivitiesLog
{
    public class ActivityLogResponseDto
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
