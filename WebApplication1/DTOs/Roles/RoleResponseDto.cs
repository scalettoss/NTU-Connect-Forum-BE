using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Roles
{
    public class RoleResponseDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
