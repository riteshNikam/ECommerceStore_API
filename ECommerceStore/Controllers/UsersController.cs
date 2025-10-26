using ECommerceStore.Data;
using ECommerceStore.DTO.User;
using ECommerceStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            if (_context.Users.Any(user => user.Email == dto.Email))
            {
                return BadRequest("User Already Exists.");
            }

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Customer"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { user_id = user.UserId, user_name = user.UserName, email = user.Email, role = user.Role });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO dto)
        {
            var token = "";
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == dto.Email);

            if ( user == null )
            {
                return NotFound("User Not Registered.");
            } else
            {
                var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

                if (validPassword) 
                {
                    token = generateJwtToken(user);
                }
                else
                {
                    return BadRequest("Incorrect Email or Password.");
                }
            }
            
            return Ok(new { user_name = user.UserName, email = user.Email, user_id = user.UserId, role = user.Role, token = token });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Unauthorized Request");
            }


            var user = await _context.Users.FindAsync(int.Parse(userId!));
            return Ok(new { user_id = user!.UserId, user_name = user!.UserName, email = user!.Email, role = user!.Role });
        }

        [HttpGet("user")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await _context.Users.Select(user => new UserDTO
            {
                UserName = user.UserName,
                UserId = user.UserId,
                Role = user.Role,
                Email = user.Email
            }).ToListAsync();

            if (userList.Count == 0)
            {
                return NoContent();
            }

            return Ok(userList);
        }

        [HttpGet("user/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUserByID(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.UserId == id);

            if ( user == null )
            {
                return NotFound("User Not Fount.");
            }

            return Ok(new { user_id = user.UserId, user_name = user.UserName, emai = user.Email, role = user.Role });
        }

        private string generateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]!)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
