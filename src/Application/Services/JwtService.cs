using Microsoft.Extensions.Options;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Services
{
    public class JwtService : IOptions<JwtSettings>
    {
        public JwtService(IOptions<JwtSettings> options)
        {
            Value = options.Value;
        }
        private JwtSettings Value { get; }
        string GenerateToken(User user)
        {
            Claim emailClaim = new Claim(ClaimTypes.Email, user.Email);
            Claim idClaim = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());


        }
    }
}