using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Users
{
    public class GetUserResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsProfilePublic { get; set; } = true;
        public DateTime? UpdatedAt { get; set; }
    }
}
