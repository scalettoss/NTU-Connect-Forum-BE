using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Bookmarks
{
    public class BookmarkResponseDto
    {
        public int BookmarkId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
