using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Repositories;

namespace DiscussifyApi.Services
{
    public class AnonymousService : IAnonymousService
    {
        private readonly IAnonymousRepository _repository;
        private readonly IMapper _mapper;

        public AnonymousService(IAnonymousRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AnonymousDto> CreateAnonymous(AnonymousCreationDto anonymous)
        {
            var anonymousModel = _mapper.Map<Anonymous>(anonymous);
            var id = await _repository.CreateAnonymous(anonymousModel);
            if(id == 0) 
            {
                return null!;
            }

            return new AnonymousDto
            {
                Id = id,
                Name = anonymousModel.Name
            };
        }

        public async Task<AnonymousDto> GetAnonymousById(int id)
        {
            var anonymousModel = await _repository.GetAnonymousById(id);
            if(anonymousModel == null)
            {
                return null!;
            }

            return new AnonymousDto
            {
                Name = anonymousModel.Name
            };
        }
    }
}