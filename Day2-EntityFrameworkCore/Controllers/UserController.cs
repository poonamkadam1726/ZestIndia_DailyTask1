using Day2_EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [HttpPost]
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
                password = loginModule.password,
                Role = "User"

            });
            myDbContext.SaveChanges();
            return Ok("User registered successfully");
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = myDbContext.Users.ToList();
            return Ok(users);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult LoginUSer(LoginModule loginModule)
        {

            var user = myDbContext.Users.FirstOrDefault(s => s.UserName == loginModule.UserName && s.password == loginModule.password);
            if (user != null)
            {
                var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim("Id",user.Id.ToString()),
              new Claim("UserName", user.UserName.ToString())

            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                 configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                 expires: DateTime.UtcNow.AddMinutes(15), //15 mins valid token
                 signingCredentials: signIn 
                   );
                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenValue, User = user });
            }
            return NoContent();
        }
    }
}
