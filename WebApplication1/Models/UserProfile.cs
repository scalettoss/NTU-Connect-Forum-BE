using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumBE.Models
{
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }
        [Required]
        public int UserId { get; set; }
        [StringLength(255)]
        public string? AvatarUrl { get; set; }
        [StringLength(500)]
        public string? Bio { get; set; }
        [StringLength(100)]
        public string? Address { get; set; }
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(20)]
        public string? Gender { get; set; }
        public bool IsProfilePublic { get; set; } = true;
        [InverseProperty("UserProfile")]
        public virtual User User { get; set; }
    }
}