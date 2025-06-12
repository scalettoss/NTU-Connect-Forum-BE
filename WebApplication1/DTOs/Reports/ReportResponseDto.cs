using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Reports
{
    public class ReportResponseDto
    {
        public int ReportId { get; set; }
        public int PostId { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        //public int HandledBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
