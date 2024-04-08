using DiscussifyApi.Controllers;
using DiscussifyApi.Dtos;
using DiscussifyApi.Hubs;
using DiscussifyApi.Models;
using DiscussifyApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language;

namespace DiscussifyApiTests.Controllers
{
    public class RoomControllerTests
    {
        private readonly RoomsController _controller;
        private readonly Mock<ILogger<RoomsController>> _logger;
        private readonly Mock<IHubContext<MessageHub, IMessageHub>> _hubContext;
        private readonly Mock<IAnonymousService> _anonymousService;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IRoomService> _roomService;
        private readonly Mock<ITokenService> _authService;
        private readonly Mock<IMessageService> _messageService;

        public RoomControllerTests()
        {
            _logger = new Mock<ILogger<RoomsController>>();
            _hubContext = new Mock<IHubContext<MessageHub, IMessageHub>> { CallBase = true };
            _anonymousService = new Mock<IAnonymousService>();
            _userService = new Mock<IUserService>();
            _roomService = new Mock<IRoomService>();
            _authService = new Mock<ITokenService> { CallBase = true };
            _messageService = new Mock<IMessageService>();  
            _controller = new RoomsController(_hubContext.Object, _anonymousService.Object, _userService.Object, _roomService.Object, _authService.Object, _messageService.Object, _logger.Object);
        }

        [Fact]
        public async Task CreateRoom_ValidRoom_ReturnsCreatedResponse()
        {
            // Arrange
            var userId = 1;
            var room = new RoomCreationDto
            {
                UserId = userId,
                Name = "Room 1"
            };

            var foundUser = new UserDto();

            _userService.Setup(u => u.GetUserById(userId)).ReturnsAsync(foundUser);
            _roomService.Setup(r => r.CreateRoom(room)).ReturnsAsync(new Room());

            // Act
            var response = await _controller.CreateRoom(room);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(response);

            var createdAtRouteResult = (CreatedAtRouteResult)response;
            Assert.Equal(StatusCodes.Status201Created, createdAtRouteResult.StatusCode);
            Assert.Equal("GetRoomById", createdAtRouteResult.RouteName);
            Assert.NotNull(createdAtRouteResult.RouteValues);
            Assert.True(createdAtRouteResult.RouteValues.ContainsKey("id"));

            var roomId = createdAtRouteResult.RouteValues["id"];
            Assert.NotNull(roomId);
            Assert.IsType<int>(roomId);
        }

        [Fact]
        public async Task CreateRoom_UserNotFound_ReturnsNotFoundResponse()
        {
            // Arrange
            var userId = 1;
            var room = new RoomCreationDto
            {
                UserId = userId,
                Name = "Room 1"
            };

            _userService.Setup(u => u.GetUserById(userId)).ReturnsAsync((UserDto)null!);

            // Act
            var response = await _controller.CreateRoom(room);

            // Assert
            Assert.IsType<ObjectResult>(response);

            var objectResult = (ObjectResult)response;
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateRoom_ExceptionThrown_ReturnsInternalServerErrorResponse()
        {
            // Arrange
            var userId = 1;
            var room = new RoomCreationDto
            {
                UserId = userId,
                Name = "Room 1"
            };

            _userService.Setup(u => u.GetUserById(userId)).ThrowsAsync(new Exception());

            // Act
            var response = await _controller.CreateRoom(room);

            // Assert
            Assert.IsType<ObjectResult>(response);

            var objectResult = (ObjectResult)response;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }

        [Fact]
        public async Task JoinRoom_RoomNotFound_ReturnsNotFound()
        {
            // Arrange
            var joinRequest = new JoinRoomDto
            {
                Code = "1234",
                Name = "sly"
            };

            _roomService.Setup(x => x.GetRoomIdByCode(joinRequest.Code)).ReturnsAsync(0);

            // Act
            var result = await _controller.JoinRoom(joinRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task JoinRoom_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var joinRequest = new JoinRoomDto
            {
                Code = "1234",
                Name = "sly"
            };

            _roomService.Setup(x => x.GetRoomIdByCode(joinRequest.Code)).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.JoinRoom(joinRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task JoinRoom_ValidRequest_ReturnsCreatedAnonymousUser()
        {
            // Arrange
            var joinRequest = new JoinRoomDto
            {
                Code = "1234",
                Name = "sly"
            };

            var foundRoom = 1;
            var newAnonymousUser = new AnonymousCreationDto
            {
                RoomId = foundRoom,
                Name = joinRequest.Name
            };
            var newAnonymous = new AnonymousDto
            {
                RoomId = foundRoom,
                Name = joinRequest.Name,
                RefreshToken = "refreshToken",
                AccessToken = "accessToken"
            };
            var authResult = new AuthenticationResult
            {
                RefreshToken = "refreshToken",
                AccessToken = "accessToken"
            };

            _roomService.Setup(x => x.GetRoomIdByCode(joinRequest.Code)).ReturnsAsync(foundRoom);
            _anonymousService.Setup(x => x.CreateAnonymous(newAnonymousUser)).ReturnsAsync(newAnonymous);
            _authService.Setup(x => x.CreateAuthToken(newAnonymous.Name, "anonymous")).ReturnsAsync(authResult);

            // Act
            var result = await _controller.JoinRoom(joinRequest);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task RefreshRoomJoinToken_WithValidRefreshToken_ReturnsOkResult()
        {
            // Arrange
            var refreshToken = "valid_refresh_token";
            var tokenRenewDto = new TokenRenewDto { RefreshToken = refreshToken };
            var expectedResult = new AuthenticationResult { AccessToken = "access_token", RefreshToken = "refresh_token" };

            _authService.Setup(x => x.VerifyRefreshToken(refreshToken, out It.Ref<string>.IsAny))
                .ReturnsAsync(true);
            _authService.Setup(x => x.CreateAuthToken(It.Ref<string>.IsAny, "anonymous"))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.RefreshRoomJoinToken(tokenRenewDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task GetAllRooms_ReturnsRooms_WhenRoomsExist()
        {
            // Arrange
            var expectedRooms = new List<RoomUserDto>()
            {
                new RoomUserDto { Id = 1, Name = "Room 1" },
                new RoomUserDto { Id = 2, Name = "Room 2" }
            };

            _roomService.Setup(service => service.GetAllRooms())
                .ReturnsAsync(expectedRooms);

            // Act
            var result = await _controller.GetAllRooms();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualRooms = Assert.IsType<List<RoomUserDto>>(okResult.Value);
            Assert.Equal(expectedRooms, actualRooms);
        }

        [Fact]
        public async Task GetRoomById_ValidId_ReturnsRoom()
        {
            // Arrange
            int roomId = 1;
            var expectedRoom = new RoomUserDto { Id = roomId, Name = "Test Room" };
            _roomService.Setup(service => service.GetRoomById(roomId)).ReturnsAsync(expectedRoom);

            // Act
            var result = await _controller.GetRoomById(roomId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(expectedRoom, okResult!.Value);
        }

        [Fact]
        public async Task UpdateRoom_ValidId_ReturnsUpdatedRoomDetails()
        {
            // Arrange
            int roomId = 1;
            RoomUpdationDto updatedRoom = new RoomUpdationDto
            {
                Name = "Updated Room"
            };
            RoomUserDto updatedRoomDetails = new RoomUserDto
            {
                Id = roomId,
                Name = "Updated Room",
            };
            _roomService.Setup(service => service.GetRoomById(roomId))
                .ReturnsAsync(updatedRoomDetails);

            // Act
            var result = await _controller.UpdateRoom(roomId, updatedRoom) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task DeleteRoom_ExistingRoom_ReturnsOkResult()
        {
            // Arrange
            int roomId = 1;
            RoomUserDto room = new RoomUserDto { Id = roomId };

            _roomService.Setup(service => service.GetRoomById(roomId)).ReturnsAsync(room);
            _roomService.Setup(service => service.DeleteRoom(roomId)).ReturnsAsync(roomId);

            // Act
            var result = await _controller.DeleteRoom(roomId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal("Room deleted successfully", okResult.Value);
        }

        [Fact]
        public async Task CreateMessage_ReturnsOkResult()
        {
            // Arrange
            int roomId = 1;
            int anonymousId = 123;
            var message = new MessageCreationDto { Content = "Niceeee Good Game" };
            var foundRoom = new RoomUserDto { Id = roomId };
            var foundAnonymous = new AnonymousDto { Id = anonymousId };
            var newMessage = new MessageGetDto {  };

            _roomService.Setup(service => service.GetRoomById(roomId)).ReturnsAsync(foundRoom);
            _anonymousService.Setup(service => service.GetAnonymousById(anonymousId)).ReturnsAsync(foundAnonymous);
            _messageService.Setup(service => service.CreateMessage(anonymousId, message)).ReturnsAsync(newMessage);
            _hubContext.Setup(hub => hub.Clients.Group($"room_{roomId}").ReceiveMessage(roomId, newMessage)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateMessage(roomId, anonymousId, message) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(newMessage, result.Value);
        }

        [Fact]
        public async Task GetAllMessages_ValidRoomId_ReturnsMessages()
        {
            // Arrange
            int roomId = 1;
            var room = new RoomUserDto { Id = roomId };
            var messages = new List<MessageGetDto>
            {
                new MessageGetDto { Id = 1, Content = "Message 1" },
                new MessageGetDto { Id = 2, Content = "Message 2" }
            };

            _roomService.Setup(r => r.GetRoomById(roomId)).ReturnsAsync(room);
            _messageService.Setup(m => m.GetAllMessages(roomId)).ReturnsAsync(messages);

            // Act
            var result = await _controller.GetAllMessages(roomId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMessages = Assert.IsType<List<MessageGetDto>>(okResult.Value);
            Assert.Equal(messages, returnedMessages);
        }


        [Fact]
        public async Task UpdateMessage_ValidIdAndMessage_ReturnsOkResult()
        {
            // Arrange
            var roomId = 1;
            var messageId = 1;
            var messageDto = new MessageUpdationDto { Content = "This is amazing!" };
            var foundRoom = new RoomUserDto { Id = roomId };
            var foundMessage = new MessageGetDto { Id = messageId };
            var updatedMessage = new MessageGetDto { Id = messageId, Content = messageDto.Content };

            _roomService.Setup(x => x.GetRoomById(roomId)).ReturnsAsync(foundRoom);
            _messageService.Setup(x => x.GetMessageById(messageId)).ReturnsAsync(foundMessage);
            _messageService.Setup(x => x.UpdateMessage(roomId, messageId, messageDto)).ReturnsAsync(updatedMessage);

            // Act
            var result = await _controller.UpdateMessage(roomId, messageId, messageDto);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var message = Assert.IsType<string>(okResult.Value);
        }

        [Fact]
        public async Task UpvoteMessage_ReturnsOkResult()
        {
            // Arrange
            int roomId = 1;
            int messageId = 1;
            int increment = 1;
            var upvotedMessage = new MessageGetDto { Id = messageId };

            _roomService.Setup(r => r.GetRoomById(roomId)).ReturnsAsync(new RoomUserDto());
            _messageService.Setup(m => m.GetMessageById(messageId)).ReturnsAsync(new MessageGetDto());
            _messageService.Setup(m => m.UpvoteMessage(messageId, increment)).ReturnsAsync(upvotedMessage);

            // Act
            var result = await _controller.UpvoteMessage(roomId, messageId, increment);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task DownvoteMessage_ValidInput_ReturnsOkResult()
        {
            // Arrange
            int roomId = 1;
            int messageId = 2;
            int decrement = 1;
            var room = new RoomUserDto { Id = roomId };
            var message = new MessageGetDto { Id = messageId };
            var downvotedMessage = new MessageGetDto { Id = messageId };
            _roomService.Setup(service => service.GetRoomById(roomId)).ReturnsAsync(room);
            _messageService.Setup(service => service.GetMessageById(messageId)).ReturnsAsync(message);
            _messageService.Setup(service => service.DownvoteMessage(messageId, decrement)).ReturnsAsync(downvotedMessage);

            // Act
            var result = await _controller.DownvoteMessage(roomId, messageId, decrement);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task DeleteMessage_ValidRoomAndMessage_ReturnsOkResult()
        {
            // Arrange
            int roomId = 1;
            int messageId = 2;
            var room = new RoomUserDto { Id = roomId };
            var message = new MessageGetDto { Id = messageId };
            _roomService.Setup(service => service.GetRoomById(roomId)).ReturnsAsync(room);
            _messageService.Setup(service => service.GetMessageById(messageId)).ReturnsAsync(message);

            // Act
            var result = await _controller.DeleteMessage(roomId, messageId);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }
    }
}
