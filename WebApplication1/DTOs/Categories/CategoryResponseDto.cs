namespace ForumBE.DTOs.Categories
{
    public class CategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }    
        public string Slug { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
        public int? CountTotalPost { get; set; }
        public bool IsDeleted { get; set; }
    }
}
