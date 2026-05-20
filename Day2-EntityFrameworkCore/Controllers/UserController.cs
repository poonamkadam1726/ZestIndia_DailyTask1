using Day2_EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

//JWT Token

namespace Day2_EntityFrameworkCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext myDbContext;
        private readonly IConfiguration configuration;

        public UserController(MyDbContext myDbContext, IConfiguration configuration)
        {
            this.myDbContext = myDbContext;
            this.configuration = configuration;
        }

        [HttpPost("UserRegistration")]
        public IActionResult UserRegistration(LoginModule loginModule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var existingUSer = myDbContext.Users.FirstOrDefault(s => s.UserName == loginModule.UserName);

            if (existingUSer != null)
            {
                return BadRequest("User already exists");
            }

            myDbContext.Users.Add(new User
            {
                UserName = loginModule.UserName,
                password = BCrypt.Net.BCrypt.HashPassword(loginModule.password),
                Role = "User"

            });
            myDbContext.SaveChanges();
            return Ok("User registered successfully");
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = myDbContext.Users.Select(u => new UserDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                Role = u.Role
            }).ToList();
            return Ok(users);

        }

        [HttpPost]
        [Route("Login")]
        public IActionResult LoginUser(LoginModule loginModule)
        {
            var user = myDbContext.Users.FirstOrDefault(
                s => s.UserName == loginModule.UserName &&
                     s.password == loginModule.password);

            if (user != null)
            {
                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Id", user.Id.ToString()),
            new Claim("UserName", user.UserName!),
            new Claim(ClaimTypes.Role, user.Role!)
        };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

                var signIn = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: claims, // IMPORTANT
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: signIn
                );

                string tokenValue = new JwtSecurityTokenHandler()
                    .WriteToken(token);

                return Ok(new
                {
                    Token = tokenValue,
                    User = user
                });
            }

            return Unauthorized("Invalid username or password");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = myDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound("USer not found");
            {
            }
            myDbContext.Users.Remove(user);
            myDbContext.SaveChanges();
            return Ok("User deleted successfully");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = myDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound ("User not found");
            }
            return Ok(new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role
            });
        }
    }
}
