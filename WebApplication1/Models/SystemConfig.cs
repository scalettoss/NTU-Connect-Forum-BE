using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ForumBE.Models
{
    public class SystemConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SystemId { get; set; }
        [Required]
        [StringLength(255)]
        public string Key { get; set; }
        [Required]
        public bool Value { get; set; }
        [Required]
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
