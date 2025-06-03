using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.UserProflies
{
    public class UserProfileResponseDto
    {
        public string AvatarUrl { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool IsProfilePublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
