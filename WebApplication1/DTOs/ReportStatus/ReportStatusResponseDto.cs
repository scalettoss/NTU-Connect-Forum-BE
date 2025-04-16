using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.ReportStatus
{
    public class ReportStatusResponseDto
    {
        public int ReportStatusId { get; set; }
        public int ReportId { get; set; }
        public string Status { get; set; }
        public bool IsProcessed { get; set; } = false;
        public int? HandledBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
