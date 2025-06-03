using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumBE.Models
{
    public class ActivityLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }
        
        [Required]
        public int UserId { get; set; }

        // created post, create comment, ... updated, deleted
        [Required]
        [StringLength(100)]
        public string Action { get; set; }

        [StringLength(1000)]
        // tạo post "abcxyz"
        public string Description { get; set; }

        // Module/tính năng nào trong hệ thống sinh ra hoạt động (ví dụ: "Post", "Auth", "User")
        [StringLength(100)]
        public string Module { get; set; }

        public DateTime? CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
} 