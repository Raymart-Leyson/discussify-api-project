using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SignalR;
using DiscussifyApi.Dtos;
using DiscussifyApi.Services;
using DiscussifyApi.Hubs;
using ASP = Microsoft.AspNetCore.Authorization;

namespace DiscussifyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IHubContext<MessageHub, IMessageHub> _hubContext;
        private readonly IAnonymousService _anonymousService;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly ITokenService _authService;
        private readonly IMessageService _messageService;
        private readonly ILogger<RoomsController> _logger;

        public RoomsController( IHubContext<MessageHub, IMessageHub> messageHubContext, IAnonymousService anonymousService, IUserService userService, IRoomService roomService, ITokenService authService, IMessageService messageService, ILogger<RoomsController> logger)
        {
            _hubContext = messageHubContext;
            _anonymousService = anonymousService;
            _userService = userService;
            _roomService = roomService;
            _authService = authService;
            _messageService = messageService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new room
        /// </summary>
        /// <param name="room">Room details</param>
        /// <returns>Returns the newly created room</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /rooms
        ///     {
        ///         "userid": 1,
        ///         "roomName: "Room 1"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Successfully created a new post</response>
        /// <response code="400">Room details invalid</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpPost(Name = "CreateRoom")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RoomCreationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRoom([FromBody] RoomCreationDto room)
        {
            try
            {
                var foundUser = await _userService.GetUserById(room.UserId);

                if (foundUser == null)
                {
                    return StatusCode(404, "User not found");
                }

                // Create the new room
                var newRoom = await _roomService.CreateRoom(room);
                return CreatedAtRoute("GetRoomById", new { id = newRoom.Id }, newRoom);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Creates a new anonymous user on join room
        /// </summary>
        /// <param name="joinRequest">Room code and alias name</param>
        /// <returns>Returns the newly created anonymous user</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /rooms/join
        ///     {
        ///         "code": "1234",
        ///         "name": "sly"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Successfully created a new anonymous user</response>
        /// <response code="400">Room details invalid</response>
        /// <response code="404">Room not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("join")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AnonymousDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> JoinRoom([FromBody] JoinRoomDto joinRequest) 
        {
            try 
            {
                var foundRoom = await _roomService.GetRoomIdByCode(joinRequest.Code!);
                if(foundRoom == 0)
                {
                    return StatusCode(404, "Room not found");
                }

                var newAnonymousUser = new AnonymousCreationDto
                {
                    RoomId = foundRoom,
                    Name = joinRequest.Name!
                };
                var newAnonymous = await _anonymousService.CreateAnonymous(newAnonymousUser);
                var authResult = await _authService.CreateAuthToken(newAnonymous.Name!, "anonymous");
                
                newAnonymous.RoomId = foundRoom;
                newAnonymous.RefreshToken = authResult.RefreshToken;
                newAnonymous.AccessToken = authResult.AccessToken;

                return Ok(newAnonymous);
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Refreshes the authentication room join token.
        /// </summary>
        /// <param name="old">The old refresh token.</param>
        /// <returns>The authentication result which contains the access token and the refresh token.</returns>
        /// <response code="200">Authentication result</response>
        /// <response code="401">Invalid refresh token</response>
        /// <response code="500">Something went wrong</response>
        [HttpPost("join/renew")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshRoomJoinToken([FromBody] TokenRenewDto old)
        {
            try
            {
                var isValid = await _authService.VerifyRefreshToken(old.RefreshToken!, out string validData);
                if(!isValid) 
                {
                    return StatusCode(401, "Invalid refresh token");
                }

                var authResult = await _authService.CreateAuthToken(validData, "anonymous");

                return Ok(authResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets all rooms
        /// </summary>
        /// <returns>Returns all rooms</returns>
        /// <response code="200">Rooms found</response>
        /// <response code="204">No rooms found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpGet(Name = "GetAllRooms")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRooms()
        {
            try
            {
                // Check if Status property was provided
                var rooms = await _roomService.GetAllRooms();

                if (rooms.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(rooms);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets the room by id
        /// </summary>
        /// <param name="id">Room id</param>
        /// <returns>Returns the details of room with id <paramref name="id"/></returns>
        /// <response code="200">Room found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Room not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpGet("{id}", Name = "GetRoomById")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoomById(int id)
        {
            try
            {
                // Check if Room exists
                var foundRoom = await _roomService.GetRoomById(id);

                if (foundRoom == null)
                {
                    return StatusCode(404, "Room not found");
                }

                return Ok(foundRoom);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Updates an existing room
        /// </summary>
        /// <param name="id">The id of the room that will be updated</param>
        /// <param name="room">New room details</param>
        /// <returns>Returns the details of room with id <paramref name="id"/></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /rooms
        ///     {
        ///         "roomName: "Room 1"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Successfully updated the room</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Room not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpPut("{id}", Name = "UpdateRoom")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RoomUpdationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomUpdationDto room)
        {
            try
            {
                // Check if room exists
                var foundRoom = await _roomService.GetRoomById(id);

                if (foundRoom == null)
                {
                    return StatusCode(404, "Room not found");
                }

                // Update the room
                await _roomService.UpdateRoom(id, room);
                return Ok("Room updated successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Deletes an existing room
        /// </summary>
        /// <param name="id">The id of the room that will be deleted</param>
        /// <response code="200">Successfully deleted the room</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Room not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpDelete("{id}", Name = "DeleteRoom")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                // Check if room exists
                var foundRoom = await _roomService.GetRoomById(id);

                if (foundRoom == null)
                {
                    return StatusCode(404, "Room not found");
                }

                await _roomService.DeleteRoom(id);
                return Ok("Room deleted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Creates a new message
        /// </summary>
        /// <param name="id">Room id</param>
        /// <param name="anonymousId">Anonymous Id</param>
        /// <param name="message">Message details</param>
        /// <returns>Returns the newly created message</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /rooms/1/messages
        ///     {
        ///         "content": "Niceeee Good Game"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Successfully created a new message</response>
        /// <response code="400">Message details are invalid</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Room not found or Anonymous not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpPost("{id}/messages", Name = "CreateMessage")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageCreationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMessage(int id, int anonymousId, [FromBody] MessageCreationDto message)
        {
            try
            {
                // Check if room exists
                var foundRoom = await _roomService.GetRoomById(id);

                if (foundRoom == null)
                {
                    return StatusCode(404, "Room not found");
                }

                var foundAnonymous = await _anonymousService.GetAnonymousById(anonymousId);

                if (foundAnonymous == null)
                {   
                    return StatusCode(404, "Anonymous not found");
                }

                // Create the new message
                var newMessage = await _messageService.CreateMessage(anonymousId, message);

                await _hubContext.Clients.Group($"room_{id}").ReceiveMessage(id, newMessage);

                return Ok(newMessage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets all messages in a room
        /// </summary>
        /// <param name="id">Room id</param>
        /// <returns>Returns all messages</returns>
        /// <response code="200">Messages found</response>
        /// <response code="204">No Messages found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Room not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpGet("{id}/messages")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMessages(int id)
        {
            try
            {
                // Check if room exists
                var foundRoom = await _roomService.GetRoomById(id);

                if (foundRoom == null)
                {
                    return StatusCode(404, "Room not found");
                }

                // Get all messages
                var messages = await _messageService.GetAllMessages(id);

                if (messages.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(messages);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Updates an existing message
        /// </summary>
        /// <param name="id">The id of the room that the message will be updated</param>
        /// <param name="messageId">The id of the message that will be updated</param>
        /// <param name="message">New message details</param>
        /// <returns>Returns the details of message with id <paramref name="id"/></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /rooms/1/messages/1
        ///     {
        ///         "content": "This is amazing!"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Successfully updated the message</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Room/Message not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpPut("{id}/messages/{messageId}", Name = "UpdateMessage")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageUpdationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMessage(int id, int messageId, [FromBody] MessageUpdationDto message)
        {
            try
            {
                // Check if room exists
                var foundRoom= await _roomService.GetRoomById(id);

                if (foundRoom == null)
                {
                    return StatusCode(404, "Room not found");
                }

                // Check if message exists
                var foundMessage = await _messageService.GetMessageById(messageId);

                if (foundMessage == null)
                {
                    return StatusCode(404, "Message not found");
                }

                // Update the message
                var updatedMessage = await _messageService.UpdateMessage(id, messageId, message);

                await _hubContext.Clients.Group($"room_{id}").ReceiveMessage(id, updatedMessage);

                return Ok("Message updated successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Upvotes a message
        /// </summary>
        /// <param name="id">The id of the room that the message will be upvoted</param>
        /// <param name="messageId">The id of the message that will be upvoted</param>
        /// <param name="increment">The amount of upvotes to be added</param>
        /// <returns>Returns the details of message with id <paramref name="id"/></returns>
        /// <response code="200">Successfully upvoted the message</response>
        /// <response code="400">Invalid request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Room or Message not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpPut("{id}/messages/{messageId}/upvote", Name = "UpvoteMessage")]
        [ProducesResponseType(typeof(MessageGetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpvoteMessage(int id, int messageId, int increment)
        {
            try
            {
                // Check if room exists
                var foundRoom= await _roomService.GetRoomById(id);

                if (foundRoom == null)
                {
                    return StatusCode(404, "Room not found");
                }

                // Check if message exists
                var foundMessage = await _messageService.GetMessageById(messageId);

                if (foundMessage == null)
                {
                    return StatusCode(404, "Message not found");
                }

                // Upvote the message
                var upvotedMessage = await _messageService.UpvoteMessage(messageId, increment);

                await _hubContext.Clients.Group($"room_{id}").ReceiveMessage(id, upvotedMessage);

                return Ok("Message upvoted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Downvotes a message
        /// </summary>
        /// <param name="id">The id of the room that the message will be upvoted</param>
        /// <param name="messageId">The id of the message that will be upvoted</param>
        /// <param name="decrement">The amount of downvotes to be added</param>
        /// <returns>Returns the details of message with id <paramref name="id"/></returns>
        /// <response code="200">Successfully downvoted the message</response>
        /// <response code="400">Invalid request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Room or Message not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpPut("{id}/messages/{messageId}/downvote", Name = "DownvoteMessage")]
        [ProducesResponseType(typeof(MessageGetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DownvoteMessage(int id, int messageId, int decrement)
        {
            try
            {
                // Check if room exists
                var foundRoom= await _roomService.GetRoomById(id);

                if (foundRoom == null)
                {
                    return StatusCode(404, "Room not found");
                }

                // Check if message exists
                var foundMessage = await _messageService.GetMessageById(messageId);

                if (foundMessage == null)
                {
                    return StatusCode(404, "Message not found");
                }

                // Upvote the message
                var downvotedMessage = await _messageService.DownvoteMessage(messageId, decrement);

                await _hubContext.Clients.Group($"room_{id}").ReceiveMessage(id, downvotedMessage);
                
                return Ok("Message downvoted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Deletes an existing message
        /// </summary>
        /// <param name="id">Room id</param>
        /// <param name="messageId">The id of the message that will be deleted</param>
        /// <response code="200">Successfully updated the message</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Room or Message not found</response>
        /// <response code="500">Internal server error</response>
        [ASP.Authorize]
        [HttpDelete("{id}/messages/{messageId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMessage(int id, int messageId)
        {
            try
            {
                // Check if room exists
                var foundRoom = await _roomService.GetRoomById(id);

                if (foundRoom == null)
                {
                    return StatusCode(404, "Room not found");
                }

                // Check if message exists
                var foundMessage = await _messageService.GetMessageById(messageId);

                if (foundMessage == null)
                {
                    return StatusCode(404, "Message not found");
                }

                await _messageService.DeleteMessage(messageId);

                await _hubContext.Clients.Group($"room_{id}").ReceiveMessage(id, messageId);
                
                return Ok("Message deleted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
