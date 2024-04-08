using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Repositories;
using DiscussifyApi.Services;
using Moq;

namespace DiscussifyApiTests.Services
{
    public class IRoomServiceTests
    {
        private readonly Mock<IRoomRepository> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly IRoomService _roomService;

        public IRoomServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _repository = new Mock<IRoomRepository>();
            _roomService = new RoomService(_repository.Object, _mapper.Object);
        }

        [Fact]
        public async Task CreateRoom_ValidRoom_ReturnsCreatedRoom()
        {
            // Arrange
            Mock<IRoomRepository> repositoryMock = new Mock<IRoomRepository>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            IRoomService roomService = new RoomService(repositoryMock.Object, mapperMock.Object);

            // Setup mapper mock
            mapperMock.Setup(m => m.Map<Room>(It.IsAny<RoomCreationDto>())).Returns(new Room());

            // Setup repository mock
            repositoryMock.Setup(r => r.GetLastRoomId()).ReturnsAsync(1);
            repositoryMock.Setup(r => r.CreateRoom(It.IsAny<Room>())).ReturnsAsync(123);

            RoomCreationDto roomDto = new RoomCreationDto();

            // Act
            Room createdRoom = await roomService.CreateRoom(roomDto);

            // Assert
            mapperMock.Verify(m => m.Map<Room>(roomDto), Times.Once);
            repositoryMock.Verify(r => r.GetLastRoomId(), Times.Once);
            repositoryMock.Verify(r => r.CreateRoom(It.IsAny<Room>()), Times.Once);

            Assert.Equal(123, createdRoom.Id);
        }

        [Fact]
        public async Task GetAllRooms_ReturnsAllRooms()
        {
            // Arrange
            Mock<IRoomRepository> repositoryMock = new Mock<IRoomRepository>();

            IRoomService roomService = new RoomService(repositoryMock.Object, null!);

            IEnumerable<RoomUserDto> expectedRooms = new List<RoomUserDto>
            {
                new RoomUserDto { Id = 1, Name = "Room 1" },
                new RoomUserDto { Id = 2, Name = "Room 2" }
            };

            repositoryMock.Setup(r => r.GetAllRooms()).ReturnsAsync(expectedRooms);

            // Act
            IEnumerable<RoomUserDto> actualRooms = await roomService.GetAllRooms();

            // Assert
            repositoryMock.Verify(r => r.GetAllRooms(), Times.Once);
            Assert.Equal(expectedRooms, actualRooms);
        }

        [Fact]
        public async Task GetRoomIdByCode_ValidCode_ReturnsRoomId()
        {
            // Arrange
            string code = "ABC123";
            int expectedRoomId = 123;
            _repository.Setup(r => r.GetRoomIdByCode(code)).ReturnsAsync(expectedRoomId);

            // Act
            int actualRoomId = await _roomService.GetRoomIdByCode(code);

            // Assert
            Assert.Equal(expectedRoomId, actualRoomId);
        }

        [Fact]
        public async Task GetRoomById_ValidId_ReturnsRoomUserDto()
        {
            // Arrange
            int roomId = 1;
            var roomUserDto = new RoomUserDto(); // Replace with your actual RoomUserDto object

            _repository
                .Setup(repo => repo.GetRoomById(roomId))
                .ReturnsAsync(roomUserDto);

            // Act
            var result = await _roomService.GetRoomById(roomId);

            // Assert
            Assert.Equal(roomUserDto, result);
            _repository.Verify(repo => repo.GetRoomById(roomId), Times.Once);
        }

        [Fact]
        public async Task UpdateRoom_ValidIdAndDto_ReturnsUpdatedRoomId()
        {
            // Arrange
            int roomId = 1;
            var roomDto = new RoomUpdationDto
            {
                Name = "Test",
            };

            var roomModel = new Room
            {
                Id = roomId,
                Name = "Test",
            };

            _mapper.Setup(mock => mock.Map<Room>(roomDto)).Returns(roomModel);
            _repository.Setup(mock => mock.UpdateRoom(roomModel)).ReturnsAsync(roomId);

            // Act
            int updatedRoomId = await _roomService.UpdateRoom(roomId, roomDto);

            // Assert
            Assert.Equal(roomId, updatedRoomId);
        }

        [Fact]
        public async Task DeleteRoom_ValidId_ReturnsDeletedRoomId()
        {
            // Arrange
            int roomId = 1;
            _repository.Setup(r => r.DeleteRoom(roomId)).ReturnsAsync(roomId);

            // Act
            int deletedRoomId = await _roomService.DeleteRoom(roomId);

            // Assert
            Assert.Equal(roomId, deletedRoomId);
        }
    }
}
