namespace ForumBE.DTOs.Users
{
    public class UserSearchRequestDto
    {
        public string Email { get; set; }
    }

    public class AdvancedUserSearchRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}