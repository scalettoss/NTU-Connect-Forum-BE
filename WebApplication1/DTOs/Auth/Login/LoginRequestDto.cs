using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Auth.Login
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
