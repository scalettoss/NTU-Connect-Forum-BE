using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Bookmarks
{
    public class BookmarkResponseDto
    {
        public int PostId { get; set; }
        public string FullName { get; set; }
        public string PostTitle { get; set; }
        public string PostSlug { get; set; }
        public string PostContent { get; set; }
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
