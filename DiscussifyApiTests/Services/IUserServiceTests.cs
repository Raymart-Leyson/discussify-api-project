using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Repositories;
using DiscussifyApi.Services;
using DiscussifyApi.Utils;
using Moq;

namespace DiscussifyApiTests.Services
{
    public class IUserServiceTests
    {
        private readonly Mock<IUserRepository> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly IUserService _userService;

        public IUserServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<IUserRepository>();
            _userService = new UserService(_repository.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateUser_Returns_UserDto()
        {
            // Arrange
            var userCreationDto = new UserCreationDto
            {
                // Set properties of userCreationDto as needed for the test
            };

            var createdUser = new User
            {
                Id = 1,
                EmailAddress = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                DateTimeCreated = DateTime.UtcNow.ToString()
            };

            var expectedUserDto = new UserDto
            {
                Id = createdUser.Id,
                EmailAddress = createdUser.EmailAddress,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                DateTimeCreated = createdUser.DateTimeCreated
            };

            _mapper.Setup(m => m.Map<User>(userCreationDto)).Returns(createdUser);
            _repository.Setup(r => r.CreateUser(createdUser)).ReturnsAsync(createdUser.Id);

            // Act
            var result = await _userService.CreateUser(userCreationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUserDto.Id, result.Id);
            Assert.Equal(expectedUserDto.EmailAddress, result.EmailAddress);
            Assert.Equal(expectedUserDto.FirstName, result.FirstName);
            Assert.Equal(expectedUserDto.LastName, result.LastName);
            Assert.Equal(expectedUserDto.DateTimeCreated, result.DateTimeCreated);
        }

        [Fact]
        public void CheckIfUserExists_UserExists_Returns1()
        {
            // Arrange
            string emailAddress = "user@example.com";
            _repository.Setup(r => r.CheckIfUserExists(emailAddress)).ReturnsAsync(1);

            // Act
            var result = _userService.CheckIfUserExists(emailAddress).Result;

            // Assert
            Assert.Equal(1, result);
            _repository.Verify(r => r.CheckIfUserExists(emailAddress), Times.Once);
        }

        [Fact]
        public void CheckIfUserExists_UserDoesNotExist_Returns0()
        {
            // Arrange
            string emailAddress = "user@example.com";
            _repository.Setup(r => r.CheckIfUserExists(emailAddress)).ReturnsAsync(0);

            // Act
            var result = _userService.CheckIfUserExists(emailAddress).Result;

            // Assert
            Assert.Equal(0, result);
            _repository.Verify(r => r.CheckIfUserExists(emailAddress), Times.Once);
        }

        [Fact]
        public async Task CheckUserEmailPasswordExists_UserExists_Returns1()
        {
            // Arrange
            var userAuthDto = new UserAuthDto
            {
                EmailAddress = "test@example.com",
                Password = "password"
            };
            var user = new User
            {
                EmailAddress = userAuthDto.EmailAddress,
                Password = Crypto.Hash(userAuthDto.Password)
            };

            _mapper.Setup(m => m.Map<User>(userAuthDto)).Returns(user);
            _repository.Setup(r => r.CheckUserEmailPasswordExists(user.EmailAddress)).ReturnsAsync(user.Password);

            // Act
            var result = await _userService.CheckUserEmailPasswordExists(userAuthDto);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetUserById_ValidId_ReturnsUserDto()
        {
            // Arrange
            int userId = 1;
            UserDto user = new UserDto { Id = userId};
            UserDto expectedDto = new UserDto { Id = userId};

            _repository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mapper.Setup(m => m.Map<UserDto>(user)).Returns(expectedDto);

            // Act
            var result = _userService.GetUserById(userId);

            // Assert
            Assert.Equal(expectedDto.Id, result.Id);
        }

        [Fact]
        public void GetUserRoomsById_WithValidId_ReturnsUserRoomDto()
        {
            // Arrange
            int userId = 1;
            var userRoomDto = new UserRoomDto();
            var expectedDto = new UserRoomDto() { Id = 1};

            _repository.Setup(repo => repo.GetUserRoomsById(userId)).ReturnsAsync(userRoomDto);

            // Act
            var result = _userService.GetUserRoomsById(userId);

            // Assert
            Assert.Equal(expectedDto.Id, result.Id);
        }

    }
}
