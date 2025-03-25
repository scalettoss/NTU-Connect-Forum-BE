using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumBE.Models
{
    public class ScamKeyword
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KeywordId { get; set; }

        [Required]
        [StringLength(100)]
        public string Keyword { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}