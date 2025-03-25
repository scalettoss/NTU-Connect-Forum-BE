using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Auth.Login
{
    public class LoginResponseDto
    {
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public int Expires { get; set; }
    }
}
