using DiscussifyApi.Services;
using Microsoft.Extensions.Configuration;
using DiscussifyApi.Models;
using System.Security.Claims;
using Moq;
using Microsoft.IdentityModel.Tokens;

namespace DiscussifyApiTests.Services
{
    public class ITokenServiceTests
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public ITokenServiceTests()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _tokenService = new TokenService(_config);
        }

        [Fact]
        public async Task CreateAuthToken_GivenDataAndType_ShouldReturnAuthenticationResult()
        {
            // Arrange
            var data = "John Doe";
            var type = "anonymous";
            
            // Action 
            var result = await _tokenService.CreateAuthToken(data, type);
            var principal = _tokenService.VerifyAccessToken(result.AccessToken!);

            // Assert
            Assert.IsType<AuthenticationResult>(result);
            Assert.NotNull(result.AccessToken);
            Assert.NotNull(result.RefreshToken);
        }

        [Fact]
        public async Task VerifyAccessToken_GivenValidToken_ShouldReturnClaimsPrincipal()
        {
            // Arrange
            var data = "John Doe";
            var type = "anonymous";
            var token = await _tokenService.CreateAuthToken(data, type);
            
            // Action 
            var principal = _tokenService.VerifyAccessToken(token.AccessToken!);

            // Assert
            Assert.IsType<ClaimsPrincipal>(principal);
        }

        [Fact]
        public async Task VerifyRefreshToken_GivenValidToken_ShouldOutValidData()
        {
            // Arrange
            var data = "John Doe";
            var type = "anonymous";
            
            
            // Action 
            var token = await _tokenService.CreateAuthToken(data, type);
            var result = _tokenService.VerifyRefreshToken(token.RefreshToken!, out var validData);

            // Assert
            Assert.Equal(data, validData);
        }

        [Fact]
        public void GenerateRefreshToken_GivenData_ShouldReturnString()
        {
            // Arrange
            var data = "John Doe";
            var type = "anonymous";
            
            // Action 
            var result = _tokenService.GenerateRefreshToken(data, type);

            // Assert
            Assert.IsType<string>(result);
        }
    }
}