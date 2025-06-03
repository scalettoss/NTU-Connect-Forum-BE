﻿namespace ForumBE.DTOs.Users
{
    public class UserChangeInforByAdminRequestDto
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
        public string? Password { get; set; }
        public bool? IsActive { get; set; }
    }
    public class AddUserByAdminRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
