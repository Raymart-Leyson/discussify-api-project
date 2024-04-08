using DiscussifyApi.Mappings;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using AutoMapper;

namespace DiscussifyApiTests.Mappings
{
    public class AnonymousMappingsTests
    {
        private readonly IMapper _mapper;

        public AnonymousMappingsTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AnonymousMapping>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void CreateMap_AnonymousCreationDtoToAnonymous_ShouldMapCorrectly()
        {
            // Arrange
            var anonymousCreationDto = new AnonymousCreationDto
            {
                Name = "John Doe"
            };

            // Act
            var anonymous = _mapper.Map<Anonymous>(anonymousCreationDto);

            // Assert
            Assert.Equal(anonymousCreationDto.Name, anonymous.Name);
        }
    }
}