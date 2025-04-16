using System.Text.Json.Serialization;

namespace ForumBE.DTOs.Users
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string FirstName { get; set; }
        [JsonIgnore]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string RoleName { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; } = true;
        [JsonIgnore]
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
