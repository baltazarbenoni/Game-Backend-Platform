using Microsoft.Extensions.Options;
using System.Security.Claims;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.Services
{
    public class JwtService
    {
        public JwtService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }
        private JwtSettings _settings { get; }
        public string GenerateToken(User user)
        {
            
            Claim emailClaim = new Claim(ClaimTypes.Email, user.Email);
            Claim idClaim = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
            var claims = new List<Claim> { emailClaim, idClaim };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
