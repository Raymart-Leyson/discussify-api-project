using DiscussifyApi.Services;
using DiscussifyApi.Repositories;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using AutoMapper;
using Moq;

namespace DiscussifyApiTests.Services
{
    public class IAnonymousServiceTests
    {
        private readonly IAnonymousService _anonymousService;
        private readonly Mock<IAnonymousRepository> _fakeAnonymousRespository;
        private readonly Mock<IMapper> _fakeMapper;

        public IAnonymousServiceTests()
        {
            _fakeAnonymousRespository = new Mock<IAnonymousRepository>();
            _fakeMapper = new Mock<IMapper>();
            _anonymousService = new AnonymousService(_fakeAnonymousRespository.Object, _fakeMapper.Object);
        }

        [Fact]
        public async Task CreateAnonymous_GivenAnonymousCreationDto_ReturnsAnonymousDto()
        {
            // Arrange
            var anonymousCreationDto = new AnonymousCreationDto
            {
                Name = "John Doe"
            };
            var anonymousModel = new Anonymous
            {
                Name = anonymousCreationDto.Name
            };
            var anonymousDto = new AnonymousDto
            {
                Id = 1,
                Name = anonymousModel.Name
            };
            _fakeMapper.Setup(mapper => mapper.Map<Anonymous>(anonymousCreationDto)).Returns(anonymousModel);
            _fakeAnonymousRespository.Setup(repository => repository.CreateAnonymous(anonymousModel)).ReturnsAsync(anonymousDto.Id);

            // Act
            var result = await _anonymousService.CreateAnonymous(anonymousCreationDto);

            // Assert
            Assert.Equal(anonymousDto.Id, result.Id);
            Assert.Equal(anonymousDto.Name, result.Name);
            Assert.Equal(anonymousDto.RefreshToken, result.RefreshToken);
            Assert.Equal(anonymousDto.AccessToken, result.AccessToken);
        }

        [Fact]
        public async Task CreateAnonymous_GivenAnonymousCreationDto_ReturnsNull()
        {
            // Arrange
            var anonymousCreationDto = new AnonymousCreationDto
            {
                Name = "John Doe"
            };
            var anonymousModel = new Anonymous
            {
                Name = anonymousCreationDto.Name
            };
            _fakeMapper.Setup(mapper => mapper.Map<Anonymous>(anonymousCreationDto)).Returns(anonymousModel);
            _fakeAnonymousRespository.Setup(repository => repository.CreateAnonymous(anonymousModel)).ReturnsAsync(0);

            // Act
            var result = await _anonymousService.CreateAnonymous(anonymousCreationDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAnonymousById_GivenAnonymousId_ReturnsAnonymousDto()
        {
            // Arrange
            var anonymousId = 1;
            var anonymousModel = new Anonymous
            {
                Name = "John Doe"
            };
            var anonymousDto = new AnonymousDto
            {
                Name = anonymousModel.Name
            };
            _fakeAnonymousRespository.Setup(repository => repository.GetAnonymousById(anonymousId)).ReturnsAsync(anonymousModel);

            // Act
            var result = await _anonymousService.GetAnonymousById(anonymousId);

            // Assert
            Assert.Equal(anonymousDto.Id, result.Id);
            Assert.Equal(anonymousDto.Name, result.Name);
            Assert.Equal(anonymousDto.AccessToken, result.AccessToken);
            Assert.Equal(anonymousDto.RefreshToken, result.RefreshToken);
        }

        [Fact]
        public async Task GetAnonymousById_NoExistingAnonymous_ReturnsNull()
        {
            // Arrange
            var anonymousId = 1;
            _fakeAnonymousRespository.Setup(repository => repository.GetAnonymousById(anonymousId)).ReturnsAsync((Anonymous)null!);

            // Act
            var result = await _anonymousService.GetAnonymousById(anonymousId);

            // Assert
            Assert.Null(result);
        }
    }
}