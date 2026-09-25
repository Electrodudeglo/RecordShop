using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RecordShop.Options;

public class LoginModel
{
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}

namespace RecordShop.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtOptions _jwt;
        public AuthController(IOptions<JwtOptions> jwtOptions)
        {
            _jwt = jwtOptions.Value;
        }

        [HttpPost("token")]
        public IActionResult GenerateToken([FromBody] LoginModel login)
        {
            if (login.Username != "mohamed@waveform.com" || login.Password != "password123") return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds);

            return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});
        }       
    }
}
