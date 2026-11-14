using CakeSystem.Application.DTOs.Auth;
using CakeSystem.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace CakeSystem.Application.Security.Jwt
{
    internal class JwtTokenServiceWithSymmetricKey : IJwtTokenService
    {
        private readonly JwtToken _jwt;

        public string Create(GetUserDto user)
        {
            var claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),

                [JwtRegisteredClaimNames.Aud] = _jwt.Audience
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.IssuerSigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int minutes = _jwt.ExpiresMinutes;

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Login),

                    new Claim("role", user.Role.ToString())
                }),

                Issuer = _jwt.Issuer,
                Expires = DateTime.UtcNow.AddMinutes(minutes),
                Claims = claims,
                SigningCredentials = creds,
            };

            var tokenString = new JsonWebTokenHandler().CreateToken(descriptor);

            return tokenString;
        }

        public JwtTokenServiceWithSymmetricKey(IOptions<JwtToken> options)
            => _jwt = options.Value;
    }
}
