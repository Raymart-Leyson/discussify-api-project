using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DiscussifyApi.Dtos;
using DiscussifyApi.Services;

namespace DiscussifyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _authService;
        private readonly ILogger<TokensController> _logger;

        public TokensController(ITokenService authService, IUserService userService, ILogger<TokensController> logger)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Acquires a new authentication token for the user.
        /// </summary>
        /// <param name="user">The user's email address and password.</param>
        /// <returns>The authentication result which contains the access token and the refresh token.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /tokens/acquire
        ///     {
        ///         "emailAddress": "user@domain",
        ///         "password": "password"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Authentication result</response>
        /// <response code="401">Invalid username or password</response>
        /// <response code="500">Something went wrong</response>
        [AllowAnonymous]
        [HttpPost("acquire")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcquireToken([FromBody] UserAuthDto user)
        {
            try 
            {
                var foundUser = await _userService.CheckUserEmailPasswordExists(user);

                if (foundUser == 0)
                {
                    return StatusCode(401, "Invalid username or password");
                }

                var authResult = await _authService.CreateAuthToken(user.EmailAddress!, "teacher");

                return Ok(authResult);
            }   
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
        
        /// <summary>
        /// Refreshes the authentication token.
        /// </summary>
        /// <param name="old">The old refresh token.</param>
        /// <returns>The authentication result which contains the access token and the refresh token.</returns>
        /// <response code="200">Authentication result</response>
        /// <response code="401">Invalid refresh token</response>
        /// <response code="500">Something went wrong</response>
        [HttpPost("renew")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRenewDto old)
        {
            try
            {
                var isValid = await _authService.VerifyRefreshToken(old.RefreshToken!, out string validUserEmail);
                if(!isValid) 
                {
                    return StatusCode(401, "Invalid refresh token");
                }

                var authResult = await _authService.CreateAuthToken(validUserEmail, "teacher");

                return Ok(authResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Verifies the refresh token.
        /// </summary>
        /// <param name="current">The refresh token to verify.</param>
        /// <returns>True if the refresh token is valid, otherwise false.</returns>
        /// <response code="200">True if the refresh token is valid, otherwise unauthorized.</response>
        /// <response code="401">Invalid refresh token</response>
        /// <response code="500">Something went wrong</response>
        [HttpPost("verify")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyRefreshToken([FromBody] TokenVerifyDto current)
        {
            try
            {
                var isValid = await _authService.VerifyRefreshToken(current.RefreshToken!, out string validUserEmail);
                if(!isValid) 
                {
                    return StatusCode(401, "Invalid refresh token");
                }

                return Ok(new { isValid });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
