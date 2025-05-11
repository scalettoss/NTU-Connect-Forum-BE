namespace ForumBE.DTOs.UserProflies
{
    public class UserProfileUpdateRequestDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile? AvatarFile { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool? IsProfilePublic { get; set; }
    }
}
