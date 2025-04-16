using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumBE.Models
{
    public class ReportStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportStatusId { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public bool IsProcessed { get; set; }

        public int? HandledBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        [ForeignKey("HandledBy")]
        public virtual User Handler { get; set; }
    }
}