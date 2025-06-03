namespace ForumBE.DTOs.Posts
{
    public class PostCreateRequestDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
