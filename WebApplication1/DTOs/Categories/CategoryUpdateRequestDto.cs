using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Categories
{
    public class CategoryUpdateRequestDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
