using DiscussifyApi.Controllers;
using DiscussifyApi.Dtos;
using DiscussifyApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace DiscussifyApiTests.Controllers
{
    public class UserControllerTests
    {
        private readonly UsersController _controller;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UsersController>> _logger;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _logger = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_mockUserService.Object, _logger.Object);
        }

        [Fact]
        public async Task CreateUser_Returns_CreatedUser()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<UsersController>>();
            var controller = new UsersController(userServiceMock.Object, loggerMock.Object);

            var user = new UserCreationDto
            {
                EmailAddress = "johndoe@gmail.com",
                Password = "strongpassword123",
                FirstName = "John",
                LastName = "Doe"
            };

            userServiceMock.Setup(x => x.CheckIfUserExists(user.EmailAddress)).ReturnsAsync(0);
            userServiceMock.Setup(x => x.CreateUser(user)).ReturnsAsync(new UserDto { Id = 1, EmailAddress = user.EmailAddress });

            // Act
            var result = await controller.CreateUser(user) as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("GetUserById", result.RouteName);
            Assert.Equal(1, actual: result.RouteValues?["id"]);
            Assert.Equal(user.EmailAddress, (result.Value as UserDto)?.EmailAddress);
        }

        [Fact]
        public async Task CreateUser_Returns_BadRequest_IfUserDetailsAreInvalid()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<UsersController>>();
            var controller = new UsersController(userServiceMock.Object, loggerMock.Object);
            controller.ModelState.AddModelError("emailAddress", "Email address is required.");
            var user = new UserCreationDto
            {
                // Invalid user details
                EmailAddress = null,
                Password = "weakpassword",
                FirstName = "",
                LastName = ""
            };

            // Act
            var result = await controller.CreateUser(user) as BadRequestResult;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUser_Returns_Conflict_IfUserExists()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<UsersController>>();
            var controller = new UsersController(userServiceMock.Object, loggerMock.Object);

            var user = new UserCreationDto
            {
                EmailAddress = "johndoe@gmail.com",
                Password = "strongpassword123",
                FirstName = "John",
                LastName = "Doe"
            };

            userServiceMock.Setup(x => x.CheckIfUserExists(user.EmailAddress)).ReturnsAsync(1);

            // Act
            var result = await controller.CreateUser(user) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(409, result.StatusCode);
            Assert.Equal("EmailAddress already exists", result.Value);
        }

        [Fact]
        public async Task CreateUser_Returns_InternalServerError_OnException()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.CheckIfUserExists(It.IsAny<string>())).ThrowsAsync(new Exception("Test exception"));

            var loggerMock = new Mock<ILogger<UsersController>>();
            var controller = new UsersController(userServiceMock.Object, loggerMock.Object);

            var user = new UserCreationDto
            {
                EmailAddress = "johndoe@gmail.com",
                Password = "strongpassword123",
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var result = await controller.CreateUser(user) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task GetUserById_ExistingId_ReturnsOkResultWithUser()
        {
            // Arrange
            int userId = 1;
            var userDto = new UserDto { Id = userId};

            _mockUserService.Setup(service => service.GetUserById(userId))
                .ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDto>(okResult.Value);

            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public async Task GetUserById_NonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            int userId = 2;
            _mockUserService.Setup(service => service.GetUserById(userId))!
                .ReturnsAsync((UserDto)null!);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetUserById_ExceptionThrown_ReturnsInternalServerErrorResult()
        {
            // Arrange
            int userId = 1;
            _mockUserService.Setup(service => service.GetUserById(userId))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task GetUserById_WithValidId_ReturnsUser()
        {
            // Arrange
            int userId = 1;
            var userDto = new UserDto { Id = userId};
            _mockUserService.Setup(service => service.GetUserById(userId)).ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(userDto, okResult.Value);
        }

        [Fact]
        public async Task Login_WithValidUser_ReturnsUser()
        {
            // Arrange
            var userAuthDto = new UserAuthDto { EmailAddress = "johndoe@mailcom", Password = "strongpassword123" };
            var foundUser = new UserDto { Id = 1};
            _mockUserService.Setup(service => service.GetUserByEmailPassword(userAuthDto)).ReturnsAsync(foundUser);

            // Act
            var result = await _controller.Login(userAuthDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(foundUser, okResult.Value);
        }

        [Fact]
        public async Task Login_WithInvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            var userAuthDto = new UserAuthDto { EmailAddress = "johndoe@mailcom", Password = "strongpassword123" };
            _mockUserService.Setup(service => service.GetUserByEmailPassword(userAuthDto)).ReturnsAsync((UserDto)null!);

            // Act
            var result = await _controller.Login(userAuthDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(401, objectResult.StatusCode);
        }

        [Fact]
        public async Task Login_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var userAuthDto = new UserAuthDto { EmailAddress = "johndoe@mailcom", Password = "strongpassword123" };
            _mockUserService.Setup(service => service.GetUserByEmailPassword(userAuthDto)).ThrowsAsync(new Exception("Some error message"));

            // Act
            var result = await _controller.Login(userAuthDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetUserRoomsById_ValidId_ReturnsUserRooms()
        {
            // Arrange
            int userId = 1;
            UserRoomDto userRooms = new UserRoomDto
            {
                Id = userId,
                Rooms = new List<RoomDto>()
            {
            new RoomDto { Id = 1, Name = "Room 1" },
            new RoomDto { Id = 2, Name = "Room 2" }
            }
            };

            _mockUserService
                .Setup(service => service.GetUserRoomsById(userId))
                .ReturnsAsync(userRooms);

            // Act
            var result = await _controller.GetUserRoomsById(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(userRooms, okResult.Value);
        }

        [Fact]
        public async Task GetUserRoomsById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            int userId = 1;

            _mockUserService
                .Setup(service => service.GetUserRoomsById(userId))
                .ReturnsAsync(new UserRoomDto { Rooms = null! });

            // Act
            var result = await _controller.GetUserRoomsById(userId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetUserRoomsById_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            int userId = 1;
            var exceptionMessage = "Something went wrong";

            _mockUserService
                .Setup(service => service.GetUserRoomsById(userId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetUserRoomsById(userId);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }
    }
}
