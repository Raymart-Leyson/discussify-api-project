using DiscussifyApi.Dtos;
using DiscussifyApi.Services;
using DiscussifyApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace DiscussifyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>Returns the newly created user</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /users
        ///     {
        ///         "emailAddress": "johndoe@gmail.com"
        ///         "password": "strongpassword123",
        ///         "firstName": "John",
        ///         "lastName": "Doe"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Successfully created a new user</response>
        /// <response code="400">User details are invalid</response>
        /// <response code="409">Username already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost(Name = "CreateUser")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserCreationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDto user)
        {
            try
            {
                // Check if username already exists
                var exists = await _userService.CheckIfUserExists(user.EmailAddress!);

                if (exists > 0)
                {
                    return StatusCode(409, "EmailAddress already exists");
                }

                // Create the new user
                var newUser = await _userService.CreateUser(user);
                return CreatedAtRoute("GetUserById", new { id = newUser.Id }, newUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Returns the details of the user</returns>
        [HttpGet("{id}", Name = "GetUserById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                if (user == null)
                {
                    return StatusCode(404, "User not found");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Login and get user details
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>Returns the details of the user</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /users/login
        ///     {
        ///         "emailAddress": "johndoe@mailcom",
        ///         "password": "strongpassword123"
        ///     }
        ///
        /// </remarks>
        [HttpPost("login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserAuthDto user)
        {
            try 
            {
                var foundUser = await _userService.GetUserByEmailPassword(user);

                if (foundUser == null)
                {
                    return StatusCode(401, "Invalid username or password");
                }

                return Ok(foundUser);
            } 
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets all rooms of user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Returns the details of rooms with userid <paramref name="id"/></returns>
        /// <response code="200">User found</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [Authorize]
        [HttpGet("{id}/rooms", Name = "GetUserRoomsById")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserRoomsById(int id)
        {
            try
            {
                // Check if user exists
                var foundUserRooms = await _userService.GetUserRoomsById(id);

                if (foundUserRooms == null)
                {
                    return StatusCode(404, "User not found");
                }

                return Ok(foundUserRooms);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}