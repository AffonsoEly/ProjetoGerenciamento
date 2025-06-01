using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjetoGerenciamento.API.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetoGerenciamento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IOptions<JwtSettings> jwtSettings) : ControllerBase
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simulação de validação e atribuição de roles
            string role;

            if (request.Username == "admin" && request.Password == "123")
            {
                role = "Admin";
            }
            else if (request.Username == "user" && request.Password == "123")
            {
                role = "Usuario";
            }
            else
            {
                return Unauthorized("Usuário ou senha inválidos");
            }

            var token = GenerateJwtToken(request.Username, role);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
