using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumBE.Models
{
    public class ScamDetection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetectionId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public bool ModelPrediction { get; set; }

        [Required]
        public float ConfidenceScore { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [Required]
        [StringLength(50)]
        public string ModelVersion { get; set; }

        public bool AdminReviewed { get; set; }

        // Navigation properties
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }
}