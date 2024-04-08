using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Mappings;
using DiscussifyApi.Models;

namespace DiscussifyApiTests.Mappings
{
    public class RoomMappingsTests
    {
        private readonly IMapper _mapper;
        public RoomMappingsTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<RoomMappings>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void RoomCreationDto_To_Room_Mapping_IsValid()
        {
            // Arrange
            var roomCreationDto = new RoomCreationDto
            {
                Name = "Something",
            };

            // Act
            var room = _mapper.Map<Room>(roomCreationDto);

            // Assert
            Assert.NotNull(room);
            Assert.Equal(roomCreationDto.Name, room.Name);
        }

        [Fact]
        public void RoomUpdationDto_To_Room_Mapping_IsValid()
        {
            // Arrange
            var roomUpdationDto = new RoomUpdationDto
            {
                Name = "Something",
            };

            // Act
            var room = _mapper.Map<Room>(roomUpdationDto);

            // Assert
            Assert.NotNull(room);
            Assert.Equal(roomUpdationDto.Name, room.Name);
        }
    }
}
