using AutoMapper;
using ForumBE.DTOs.Auth.Register;
using ForumBE.Middlewares.ErrorHandling;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace ForumBE.Auth.Register
{
    public class RegisterService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        public RegisterService(IUserService userService, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto input)
        {

            if (!Regex.IsMatch(input.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new HandleException("Invalid email format.", 400);
            }

            if (input.Password.Length < 6)
            {
                throw new HandleException("Password must be at least 6 characters long.", 400);
            }

            var userExist = await _userService.GetUserByEmailAsync(input.Email);

            if (userExist != null)
            {
                throw new HandleException("Conflict: Email already exists.", 409);
            }

            if (input.ConfirmPassword != input.Password)
            {
                throw new HandleException("ConfirmPassword not match.", 400);
            }

            var hashedPassword = _passwordHasher.HashPassword(null, input.Password);

            var user = new User
            {
                Email = input.Email,
                PasswordHash = hashedPassword,
                FirstName = input.FirstName,
                LastName = input.LastName,
                RoleId = 1,
            };

            await _userService.AddUserAsync(user);

            return new RegisterResponseDto
            {
                Message = "User registered successfully",
                Code = 201
            };
        }
    }
}
