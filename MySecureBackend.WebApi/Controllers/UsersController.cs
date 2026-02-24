using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Models.DTOs;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public UsersController(UserRepository userRepository, PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password required.");

            if (request.Password.Length < 8)
                return BadRequest("Password must be at least 8 characters.");

            var existingUser = await _userRepository.GetByUserName(request.UserName);
            if (existingUser != null)
                return BadRequest("Username already exists.");

            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = _passwordService.HashPassword(request.Password)
            };

            await _userRepository.Create(user);

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByUserName(request.UserName);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            bool validPassword = _passwordService.VerifyPassword(request.Password, user.PasswordHash);

            if (!validPassword)
                return Unauthorized("Invalid username or password.");

            return Ok("Login successful.");
        }
    }
}