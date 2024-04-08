using DiscussifyApi.Mappings;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using AutoMapper;

namespace DiscussifyApiTests.Mappings
{
    public class MessageMappingsTests
    {
        private readonly IMapper _mapper;

        public MessageMappingsTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MessageMapping>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void CreateMap_MessageCreationDtoToMessage_ShouldMapCorrectly()
        {
            // Arrange
            var messageCreationDto = new MessageCreationDto
            {
                Content = "Content"
            };

            // Act
            var message = _mapper.Map<Message>(messageCreationDto);

            // Assert
            Assert.Equal(messageCreationDto.Content, message.Content);
        }

        [Fact]
        public void CreateMap_MessageUpdationDtoToMessage_ShouldMapCorrectly()
        {
            // Arrange
            var messageUpdationDto = new MessageUpdationDto
            {
                Content = "Content"
            };

            // Act
            var message = _mapper.Map<Message>(messageUpdationDto);

            // Assert
            Assert.Equal(messageUpdationDto.Content, message.Content);
        }
    }
}