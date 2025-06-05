using Microsoft.AspNetCore.Mvc;
using Notify.DTOs;
using Notify.Interfaces;
using Notify.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Notify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;

        public AuthController(IUserRepository userRepo, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            if (await _userRepo.GetByUsernameAsync(dto.Username) != null)
                return BadRequest("Username already exists.");

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Username = dto.Username,
                Role = dto.Role,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };

            await _userRepo.Add(user);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _userRepo.GetByUsernameAsync(dto.Username);
            if (user == null) return Unauthorized("Invalid username.");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for (int i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password.");

            var token = _tokenService.CreateToken(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }
    }
}
