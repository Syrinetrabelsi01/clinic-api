using ClinicAPI.DTOs;
using ClinicAPI.Helpers;
using ClinicAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            // 🔒 Create a new AppUser instance
            var user = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                Role = dto.Role
            };

            // 🔑 Attempt to create the user with the provided password
            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Password is required" });

            var result = await _userManager.CreateAsync(user, dto.Password);


            if (!result.Succeeded)
            {
                // Return detailed validation errors (e.g., weak password, duplicate email)
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new
                {
                    message = "Registration failed",
                    errors = errors
                });
            }

            // ✅ Add the user to the specified role (Doctor, Receptionist, etc.)
            if (string.IsNullOrWhiteSpace(dto.Role))
                return BadRequest(new { message = "Role is required" });

            await _userManager.AddToRoleAsync(user, dto.Role);


            // 🔐 Generate and return a JWT token
            return Ok(new
            {
                message = "Registration successful",
                token = _tokenService.CreateToken(user)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required" });

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound(new { message = "User not found" });

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Password is required" });

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return Unauthorized(new { message = "Invalid password" });

            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                message = "Login successful",
                token = token
            });
        }
    }
}
