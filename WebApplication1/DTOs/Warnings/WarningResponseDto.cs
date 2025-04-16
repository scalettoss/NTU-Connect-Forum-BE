using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Warnings
{
    public class WarningResponseDto
    {
        public int WarningId { get; set; }
        public int UserId { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public int IssuedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
