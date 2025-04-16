using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.UserProflies
{
    public class UserProfileResponseDto
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string AvatarUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public bool IsProfilePublic { get; set; } = true;
        public DateTime? UpdatedAt { get; set; }
    }
}
