using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Reports
{
    public class ReportResponseDto
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
