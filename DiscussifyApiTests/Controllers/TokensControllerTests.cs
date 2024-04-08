using DiscussifyApi.Controllers;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace DiscussifyApiTests.Controllers
{
    public class TokensControllerTests
    {
        private readonly TokensController _tokensController;
        private readonly Mock<ITokenService> _fackTokenService;
        private readonly Mock<IUserService> _fakeUserService;
        private readonly Mock<ILogger<TokensController>> _loggerMock;

        public TokensControllerTests()
        {
            _fackTokenService = new Mock<ITokenService>();
            _fakeUserService = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<TokensController>>();
            _tokensController = new TokensController(_fackTokenService.Object, _fakeUserService.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AcquireToken_ValidUser_ReturnsAuthenticationResult()
        {
            // Arrange
            var user = new UserAuthDto
            {
                EmailAddress = "user@domain",
                Password = "password"
            };

            _fakeUserService.Setup(x => x.CheckUserEmailPasswordExists(user)).ReturnsAsync(1);

            var authResult = new AuthenticationResult
            {
                AccessToken = "access_token",
                RefreshToken = "refresh_token"
            };

            _fackTokenService.Setup(x => x.CreateAuthToken(user.EmailAddress, "teacher")).ReturnsAsync(authResult);

            // Act
            var result = await _tokensController.AcquireToken(user);

            // Assert
            var statusCodeResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);

            var response = Assert.IsType<OkObjectResult>(result);
            var authenticationResult = Assert.IsType<AuthenticationResult>(response.Value);
            Assert.Equal(authResult.AccessToken, authenticationResult.AccessToken);
            Assert.Equal(authResult.RefreshToken, authenticationResult.RefreshToken);
        }

        [Fact]
        public async Task AcquireToken_InvalidUser_ReturnsUnauthorizedStatus()
        {
            // Arrange
            var user = new UserAuthDto
            {
                EmailAddress = "user@domain",
                Password = "password"
            };

            _fakeUserService.Setup(x => x.CheckUserEmailPasswordExists(user)).ReturnsAsync(0);

            // Act
            var result = await _tokensController.AcquireToken(user);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, statusCodeResult.StatusCode);
            Assert.Equal("Invalid username or password", statusCodeResult.Value);
        }

        [Fact]
        public async Task AcquireToken_ExceptionThrown_ReturnsInternalServerErrorStatus()
        {
            // Arrange
            var user = new UserAuthDto
            {
                EmailAddress = "user@domain",
                Password = "password"
            };

            _fakeUserService.Setup(x => x.CheckUserEmailPasswordExists(user)).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _tokensController.AcquireToken(user);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Something went wrong", statusCodeResult.Value);
        }
        
        [Fact]
        public async Task RefreshToken_ValidRefreshToken_ReturnsAuthenticationResult()
        {
            // Arrange
            var tokenRenewDto = new TokenRenewDto { RefreshToken = "valid_refresh_token" };
            var validUserEmail = "test@example.com";
            var authResult = new AuthenticationResult { AccessToken = "access_token", RefreshToken = "new_refresh_token" };

            _fackTokenService
                .Setup(authService => authService.VerifyRefreshToken(tokenRenewDto.RefreshToken, out validUserEmail))
                .ReturnsAsync(true);

            _fackTokenService
                .Setup(authService => authService.CreateAuthToken(validUserEmail, "teacher"))
                .ReturnsAsync(authResult);

            // Act
            var result = await _tokensController.RefreshToken(tokenRenewDto);

            // Assert
            var statusCodeResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);

            var authenticationResult = Assert.IsType<AuthenticationResult>(statusCodeResult.Value);
            Assert.Equal(authResult.AccessToken, authenticationResult.AccessToken);
            Assert.Equal(authResult.RefreshToken, authenticationResult.RefreshToken);
        }

        [Fact]
        public async Task RefreshToken_InvalidRefreshToken_ReturnsUnauthorizedStatus()
        {
            // Arrange
            var tokenRenewDto = new TokenRenewDto { RefreshToken = "invalid_refresh_token" };
            var validUserEmail = "";
            
            _fackTokenService
                .Setup(authService => authService.VerifyRefreshToken(tokenRenewDto.RefreshToken, out validUserEmail))
                .ReturnsAsync(false);

            // Act
            var result = await _tokensController.RefreshToken(tokenRenewDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, statusCodeResult.StatusCode);
            Assert.Equal("Invalid refresh token", statusCodeResult.Value);
        }

        [Fact]
        public async Task RefreshToken_ExceptionThrown_ReturnsInternalServerErrorStatus()
        {
            // Arrange
            var tokenRenewDto = new TokenRenewDto { RefreshToken = "valid_refresh_token" };
            var validUserEmail = "test@example.com";
            
            _fackTokenService
                .Setup(authService => authService.VerifyRefreshToken(tokenRenewDto.RefreshToken, out validUserEmail))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _tokensController.RefreshToken(tokenRenewDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Something went wrong", statusCodeResult.Value);
        }

        [Fact]
        public async Task VerifyRefreshToken_ValidRefreshToken_ShouldReturnOk()
        {
            // Arrange
            var tokenVerifyDto = new TokenVerifyDto { RefreshToken = "valid_refresh_token" };
            var validUserEmail = "test@example.com";

            _fackTokenService
                .Setup(authService => authService.VerifyRefreshToken(tokenVerifyDto.RefreshToken, out validUserEmail))
                .ReturnsAsync(true);

            // Act
            var result = await _tokensController.VerifyRefreshToken(tokenVerifyDto);

            // Assert
            var statusCodeResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, statusCodeResult.StatusCode);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task VerifyRefreshToken_InvalidRefreshToken_ReturnsUnauthorizedStatus()
        {
            // Arrange
            var tokenVerifyDto = new TokenVerifyDto { RefreshToken = "invalid_refresh_token" };
            var validUserEmail = "";

            _fackTokenService
                .Setup(authService => authService.VerifyRefreshToken(tokenVerifyDto.RefreshToken, out validUserEmail))
                .ReturnsAsync(false);

            // Act
            var result = await _tokensController.VerifyRefreshToken(tokenVerifyDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, statusCodeResult.StatusCode);
            Assert.Equal("Invalid refresh token", statusCodeResult.Value);
        }
        
        [Fact]
        public async Task VerifyRefreshToken_ExceptionThrown_ReturnsInternalServerErrorStatus()
        {
            // Arrange
            var tokenVerifyDto = new TokenVerifyDto { RefreshToken = "valid_refresh_token" };
            var validUserEmail = "test@example.com";

            _fackTokenService
                .Setup(authService => authService.VerifyRefreshToken(tokenVerifyDto.RefreshToken, out validUserEmail))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _tokensController.VerifyRefreshToken(tokenVerifyDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Something went wrong", statusCodeResult.Value);
        }
    }
}