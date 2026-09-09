using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TatumBackendProject.Entities;

namespace TatumBackendProject.Auth
{
    public class JwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(
            IOptions<JwtSettings> options) 
        {
            _settings = options.Value;

            if (string.IsNullOrWhiteSpace(
                _settings.Secret))
            {
                throw new InvalidOperationException(
                    "JWT Secret is not configured.");
            }
        }

        public string GenerateAccessToken(
            User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));

            var credentials = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256);

            var claims =
                new List<Claim>
                { 
                    // Standard JWT sbject
                    new(
                        JwtRegisteredClaimNames.Sub,
                        user.Id.ToString()),

                    // Email
                    new(
                        JwtRegisteredClaimNames.Email,
                        user.Email),

                    // ASP.Net Core user ID
                    new(
                        ClaimTypes.NameIdentifier,
                        user.Id.ToString()),

                    // ASP.Net Core user Email
                    new(
                        ClaimTypes.Email,
                        user.Email),

                    // ASP.Net Core role
                    new(
                        ClaimTypes.Role,
                        user.Role),
                };

            var token =
                new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_settings.AccessTokenMinutes),
                    signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
