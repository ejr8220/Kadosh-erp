using Application.Interfaces;
using Domain.Entities.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services.Security
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string token, DateTime expiresAt) CreateAccessToken(User user)
        {
            var issuer = _configuration["Jwt:Issuer"] ?? "KadoshERP";
            var audience = _configuration["Jwt:Audience"] ?? "KadoshERP.Client";
            var key = _configuration["Jwt:Key"] ?? "CHANGE_ME_SUPER_SECRET_KEY_1234567890";
            var expiresMinutes = int.TryParse(_configuration["Jwt:AccessTokenMinutes"], out var minutes)
                ? minutes
                : 60;

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.UserCode),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }

        public string CreateRefreshTokenValue()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string CreatePasswordResetTokenValue()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
        }
    }
}
