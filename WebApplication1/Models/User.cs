using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumBE.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public int RoleId { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation property
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [InverseProperty("User")]
        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ReportStatus> HandledReports { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<Warning> ReceivedWarnings { get; set; }
        public virtual ICollection<Warning> IssuedWarnings { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}