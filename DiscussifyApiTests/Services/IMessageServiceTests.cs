using DiscussifyApi.Services;
using DiscussifyApi.Repositories;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using AutoMapper;
using Moq;

namespace DiscussifyApiTests.Services
{
    public class IMessageServiceTests
    {
        private readonly IMessageService _messageService;
        private readonly Mock<IMessageRepository> _fakeMessageRepository;
        private readonly Mock<IMapper> _fakeMapper;

        public IMessageServiceTests()
        {
            _fakeMessageRepository = new Mock<IMessageRepository>();
            _fakeMapper = new Mock<IMapper>();
            _messageService = new MessageService(_fakeMessageRepository.Object, _fakeMapper.Object);
        }

        [Fact]
        public async Task CreateMessage_ValidAnounymousIdAndMessage_ShouldReturnsMessageGetDto()
        {
            // Arrange
            _fakeMapper.Setup(mapper => mapper.Map<Message>(It.IsAny<MessageCreationDto>())).Returns(new Message());
            _fakeMessageRepository.Setup(repository => repository.CreateMessage(It.IsAny<Message>())).ReturnsAsync(1);
            _fakeMessageRepository.Setup(repository => repository.GetMessageById(It.IsAny<int>())).ReturnsAsync(new MessageGetDto());

            // Act
            var result = await _messageService.CreateMessage(1, It.IsAny<MessageCreationDto>());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MessageGetDto>(result);
        }

        [Fact]
        public async Task GetAllMessages_ShouldReturnMessages()
        {
            // Arrange
            int id = 123; // Example ID
            var expectedMessages = new List<MessageGetDto>
            {
                new MessageGetDto(),
                new MessageGetDto(),
                new MessageGetDto()
            };

            _fakeMessageRepository
                .Setup(repository => repository.GetAllMessages(id))
                .ReturnsAsync(expectedMessages);

            // Act
            var result = await _messageService.GetAllMessages(id);

            // Assert
            Assert.Equal(expectedMessages, result);
        }

        [Fact]
        public async Task GetAllMessages_NoMessages_ShouldReturnEmptyList()
        {
            // Arrange
            int id = 123; // Example ID
            var expectedMessages = new List<MessageGetDto>();

            _fakeMessageRepository
                .Setup(repository => repository.GetAllMessages(id))
                .ReturnsAsync(expectedMessages);

            // Act
            var result = await _messageService.GetAllMessages(id);

            // Assert
            Assert.Equal(expectedMessages, result);
        }

        [Fact]
        public async Task GetMessageById_ValidId_ShouldReturnMessageGetDto()
        {
            // Arrange
            int id = 123; // Example ID
            var expectedMessage = new MessageGetDto { Id = id };

            _fakeMessageRepository
                .Setup(repository => repository.GetMessageById(id))
                .ReturnsAsync(expectedMessage);

            // Act
            var result = await _messageService.GetMessageById(id);

            // Assert
            Assert.Equal(expectedMessage, result);
            Assert.IsType<MessageGetDto>(result);
        }

        [Fact]
        public async Task GetMessageById_NoExistingMessage_ShouldReturnNull()
        {
            // Arrange
            int id = 123; // Example ID
            MessageGetDto expectedMessage = null!;

            _fakeMessageRepository
                .Setup(repository => repository.GetMessageById(id))!
                .ReturnsAsync(expectedMessage);

            // Act
            var result = await _messageService.GetMessageById(id);

            // Assert
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public async Task UpdateMessage_ValidMessage_ShouldReturnMessageGetDto()
        {
            // Arrange
            int anonymousId = 123; // Example anonymous ID
            int messageId = 456; // Example message ID
            var messageUpdationDto = new MessageUpdationDto { Content = "Updated message" };

            var expectedMessage = new MessageGetDto { Id = messageId, Content = "Updated message" };
            var mappedMessage = new Message { Id = messageId, Content = "Updated message", AnonymousId = anonymousId };

            _fakeMapper
                .Setup(mapper => mapper.Map<Message>(messageUpdationDto))
                .Returns(mappedMessage);

            _fakeMessageRepository
                .Setup(repository => repository.GetMessageById(messageId))
                .ReturnsAsync(expectedMessage);

            // Act
            var result = await _messageService.UpdateMessage(anonymousId, messageId, messageUpdationDto);

            // Assert
            Assert.Equal(expectedMessage, result);
            _fakeMessageRepository.Verify(repository => repository.UpdateMessage(mappedMessage), Times.Once);
            Assert.IsType<MessageGetDto>(result);
        }

        // this is failed please fix it
        [Fact]
        public async Task UpdateMessage_InvalidMessage_ShouldReturnNull()
        {
            // Arrange
            int anonymousId = 123; // Example anonymous ID
            int messageId = 456; // Example message ID
            var messageUpdationDto = new MessageUpdationDto { Content = "Updated message" };

            _fakeMapper
                .Setup(mapper => mapper.Map<Message>(messageUpdationDto))
                .Returns(new Message { Id = messageId, Content = "Updated message", AnonymousId = anonymousId });

            _fakeMessageRepository
                .Setup(repository => repository.GetMessageById(messageId))
                .ReturnsAsync((MessageGetDto)null!);

            // Act
            var result = await _messageService.UpdateMessage(anonymousId, messageId, messageUpdationDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpvoteMessage_ValidMessageId_ShouldReturnMessageGetDto()
        {
            // Arrange
            int messageId = 123; // Example message ID
            int increment = 1; // Example increment
            var expectedMessage = new MessageGetDto { Id = messageId, Votes = 1 };

            _fakeMessageRepository
                .Setup(repository => repository.UpvoteMessage(messageId, increment))
                .ReturnsAsync(expectedMessage);

            // Act
            var result = await _messageService.UpvoteMessage(messageId, increment);

            // Assert
            Assert.Equal(expectedMessage, result);
            Assert.IsType<MessageGetDto>(result);
        }

        [Fact]
        public async Task UpvoteMessage_InvalidMessageId_ShouldReturnNull()
        {
            // Arrange
            int messageId = 123; // Example message ID
            int increment = 1; // Example increment

            _fakeMessageRepository
                .Setup(repository => repository.UpvoteMessage(messageId, increment))
                .ReturnsAsync((MessageGetDto)null!);

            // Act
            var result = await _messageService.UpvoteMessage(messageId, increment);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpvoteMessage_ValidIncrement_ShouldReturnMessageGetDto()
        {
            // Arrange
            int messageId = 123; // Example message ID
            int increment = 1; // Example increment
            var expectedMessage = new MessageGetDto { Id = messageId, Votes = 1 };

            _fakeMessageRepository
                .Setup(repository => repository.UpvoteMessage(messageId, increment))
                .ReturnsAsync(expectedMessage);

            // Act
            var result = await _messageService.UpvoteMessage(messageId, increment);

            // Assert
            Assert.Equal(expectedMessage, result);
            Assert.IsType<MessageGetDto>(result);
        }

        [Fact]
        public async Task UpvoteMessage_InvalidIncrement_ShouldReturnNull()
        {
            // Arrange
            int messageId = 123; // Example message ID
            int increment = 1; // Example increment

            _fakeMessageRepository
                .Setup(repository => repository.UpvoteMessage(messageId, increment))
                .ReturnsAsync((MessageGetDto)null!);

            // Act
            var result = await _messageService.UpvoteMessage(messageId, increment);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DownvoteMessage_ValidMessageId_ShouldReturnMessageGetDto()
        {
            // Arrange
            int messageId = 123; // Example message ID
            int increment = -1; // Example increment
            var expectedMessage = new MessageGetDto { Id = messageId, Votes = -1 };

            _fakeMessageRepository
                .Setup(repository => repository.DownvoteMessage(messageId, increment))
                .ReturnsAsync(expectedMessage);

            // Act
            var result = await _messageService.DownvoteMessage(messageId, increment);

            // Assert
            Assert.Equal(expectedMessage, result);
            Assert.IsType<MessageGetDto>(result);
        }

        [Fact]
        public async Task DownvoteMessage_InvalidMessageId_ShouldReturnNull()
        {
            // Arrange
            int messageId = 123; // Example message ID
            int increment = -1; // Example increment

            _fakeMessageRepository
                .Setup(repository => repository.DownvoteMessage(messageId, increment))
                .ReturnsAsync((MessageGetDto)null!);

            // Act
            var result = await _messageService.DownvoteMessage(messageId, increment);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DownvoteMessage_ValidIncrement_ShouldReturnMessageGetDto()
        {
            // Arrange
            int messageId = 123; // Example message ID
            int increment = -1; // Example increment
            var expectedMessage = new MessageGetDto { Id = messageId, Votes = -1 };

            _fakeMessageRepository
                .Setup(repository => repository.DownvoteMessage(messageId, increment))
                .ReturnsAsync(expectedMessage);

            // Act
            var result = await _messageService.DownvoteMessage(messageId, increment);

            // Assert
            Assert.Equal(expectedMessage, result);
            Assert.IsType<MessageGetDto>(result);
        }

        [Fact]
        public async Task DownvoteMessage_InvalidIncrement_ShouldReturnNull()
        {
            // Arrange
            int messageId = 123; // Example message ID
            int increment = -1; // Example increment

            _fakeMessageRepository
                .Setup(repository => repository.DownvoteMessage(messageId, increment))
                .ReturnsAsync((MessageGetDto)null!);

            // Act
            var result = await _messageService.DownvoteMessage(messageId, increment);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteMessage_ValidMessageId_ShouldId()
        {
            // Arrange
            int messageId = 123; // Example message ID

            _fakeMessageRepository
                .Setup(repository => repository.DeleteMessage(messageId))
                .ReturnsAsync(messageId);

            // Act
            var result = await _messageService.DeleteMessage(messageId);

            // Assert
            Assert.Equal(messageId, result);
        }

        [Fact]
        public async Task DeleteMessage_InvalidMessageId_ShouldReturnZero()
        {
            // Arrange
            int messageId = 123; // Example message ID

            _fakeMessageRepository
                .Setup(repository => repository.DeleteMessage(messageId))
                .ReturnsAsync(0);

            // Act
            var result = await _messageService.DeleteMessage(messageId);

            // Assert
            Assert.Equal(0, result);
        }
    }
}