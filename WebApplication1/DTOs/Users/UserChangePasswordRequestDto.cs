namespace ForumBE.DTOs.Users
{
    public class UserChangePasswordRequestDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
