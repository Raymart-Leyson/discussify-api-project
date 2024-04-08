using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Mappings;
using DiscussifyApi.Models;

namespace DiscussifyApiTests.Mappings
{
    public class UserMappingsTests
    {
        private readonly IMapper _mapper;
        public UserMappingsTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<UserMappings>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void UserCreationDto_To_User_Mapping_IsValid()
        {
            // Arrange
            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Password = "password123"
            };

            // Act
            var user = _mapper.Map<User>(userCreationDto);

            // Assert
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            // Add more assertions based on your mapping logic
        }

        [Fact]
        public void UserUpdationDto_To_User_Mapping_IsValid()
        {
            // Arrange
            var userUpdationDto = new UserUpdationDto
            {
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var user = _mapper.Map<User>(userUpdationDto);

            // Assert
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            // Add more assertions based on your mapping logic
        }

        [Fact]
        public void UserAuthDto_To_User_Mapping_IsValid()
        {
            // Arrange
            var userAuthDto = new UserAuthDto
            {
                EmailAddress = "john@example.com",
                Password = "password123"
            };

            // Act
            var user = _mapper.Map<User>(userAuthDto);

            // Assert
            Assert.Equal("john@example.com", user.EmailAddress);
            // Add more assertions based on your mapping logic
        }
    }
}
