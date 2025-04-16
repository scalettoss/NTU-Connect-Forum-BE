namespace ForumBE.DTOs.Users
{
    public class UserUpdateProfilesRequestDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUrl { get; set; } = "/images/default_avatar.png";
        public IFormFile? AvatarFile { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool? IsProfilePublic { get; set; } = true;
    }
}
