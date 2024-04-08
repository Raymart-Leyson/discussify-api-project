using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Concurrent;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;

namespace DiscussifyApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private static readonly ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>();

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public Task<AuthenticationResult> CreateAuthToken(string data, string type)
        {
            var issuer = _config["Jwt:ISSUER"];
            var audience = _config["Jwt:AUDIENCE"];
            var key = _config["Jwt:ACCESS_SECRET_KEY"];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha512Signature
            );

            ClaimsIdentity subject = null!;

            if(type == "teacher")
            {
                subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, data),
                    new Claim(JwtRegisteredClaimNames.Email, data),
                    new Claim(JwtRegisteredClaimNames.Typ, type),
                });
            }
            else if (type == "anonymous")
            {
                subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, data),
                    new Claim(JwtRegisteredClaimNames.Name, data),
                    new Claim(JwtRegisteredClaimNames.Typ, type),
                });
            }
            

            var tokenExpires = DateTime.UtcNow.AddMinutes(10);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Issuer = issuer,
                Audience = audience,
                Expires = tokenExpires,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Task.FromResult(new AuthenticationResult
            {
                AccessToken = jwtToken,
                RefreshToken = GenerateRefreshToken(data, type)
            });
        }

        public ClaimsPrincipal VerifyAccessToken(string accessToken)
        {
            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal is null)
            {
                return null!;
            }

            var validUserEmail = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(validUserEmail))
            {
                return null!;
            }

            return principal;
        }

        public Task<bool> VerifyRefreshToken(string refreshToken, out string validData) 
        {
            // scan in the dictionary for the refresh token
            validData = _refreshTokens.FirstOrDefault(x => x.Value == refreshToken).Key;
            if (string.IsNullOrEmpty(validData))
            {
                validData = string.Empty;
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:ACCESS_SECRET_KEY"]!)),
                ValidateLifetime = false,
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase) || (jwtSecurityToken.ValidTo < DateTime.UtcNow))
            {
                return null!;
            }

            return principal;
        }

        public string GenerateRefreshToken(string data, string type)
        {
            var key = _config["Jwt:REFRESH_SECRET_KEY"];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha512Signature
            );

            ClaimsIdentity subject = null!;

            if (type == "teacher")
            {
                subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, data),
                    new Claim(JwtRegisteredClaimNames.Email, data),
                    new Claim(JwtRegisteredClaimNames.Typ, type),
                });
            }
            else if (type == "anonymous")
            {
                subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, data),
                    new Claim(JwtRegisteredClaimNames.Name, data),
                    new Claim(JwtRegisteredClaimNames.Typ, type),
                });
            }

            var tokenExpires = DateTime.UtcNow.AddDays(1);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = tokenExpires,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = _refreshTokens.AddOrUpdate(data, jwtToken, (key, oldValue) => jwtToken);

            return refreshToken;
        }
    }
}