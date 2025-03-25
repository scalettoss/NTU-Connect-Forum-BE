using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumBE.Models
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? PostId { get; set; }

        public int? CommentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Reason { get; set; }

        public DateTime? ReportedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }

        public virtual ReportStatus ReportStatus { get; set; }
    }
}