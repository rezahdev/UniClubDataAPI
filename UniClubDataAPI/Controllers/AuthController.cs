using Microsoft.AspNetCore.Mvc;
using UniClubDataAPI.Models;
using UniClubDataAPI.Models.Dto;
using UniClubDataAPI.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UniClubDataAPI.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDBContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody]LoginCredentialsDTO credentials)
        {
            UserDTO userDTO = await IsValidUser(credentials);

            if (userDTO != null)
            {
                return Ok(GenerateJwtToken(userDTO));
            }
            return Unauthorized("Invalid email or password");
        }

        private async Task<UserDTO> IsValidUser(LoginCredentialsDTO credentials)
        {
            User user = await _db.Users.SingleAsync(u => u.Email == credentials.Email);

            if(user == null)
            {
                return null;
            }

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            PasswordVerificationResult passwordVerification = 
                passwordHasher.VerifyHashedPassword(user, user.Password, credentials.Password);
            
            if(passwordVerification == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return new UserDTO(user);
        }

        private string GenerateJwtToken(UserDTO userDTO)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Name", userDTO.Name),
                new Claim(ClaimTypes.Email, userDTO.Email),
                new Claim("AccessLevel", userDTO.AccessLevel.ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
