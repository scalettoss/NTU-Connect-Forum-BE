using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumBE.Models
{
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }

        public int? PostId { get; set; }

        public int? CommentId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string FileType { get; set; }

        [Required]
        public string FileSize { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatdedAt { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}