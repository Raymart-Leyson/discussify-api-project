using AutoMapper;
using DiscussifyApi.Utils;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Repositories;

namespace DiscussifyApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUser(UserCreationDto user)
        {
            var model = _mapper.Map<User>(user);
            model.Id = await _repository.CreateUser(model);

            var newUser = new UserDto
            {
                Id = model.Id,
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateTimeCreated = model.DateTimeCreated,
            };

            return newUser;
        }

        public async Task<UserDto> GetUserByEmailPassword(UserAuthDto user)
        {
            var model = _mapper.Map<User>(user);
            var auth =  await _repository.CheckUserEmailPasswordExists(model.EmailAddress!);
            
            if (auth == null) 
                return null!;

            if(Crypto.VerifyPassword(model.Password!, auth))
            {
                return await _repository.GetUserByEmail(model.EmailAddress!);
            }

            return null!;
        }

        public Task<int> CheckIfUserExists(string emailAddress)
        {
            return _repository.CheckIfUserExists(emailAddress);
        }

        public async Task<int> CheckUserEmailPasswordExists(UserAuthDto user)
        {   
            var model = _mapper.Map<User>(user);
            var auth =  await _repository.CheckUserEmailPasswordExists(model.EmailAddress ?? string.Empty);
            
            if (auth == null) 
                return 0;

            if(Crypto.VerifyPassword(model.Password!, auth))
                return 1;

            return 0;
        }

        public Task<UserDto> GetUserById(int id)
        {
            return _repository.GetUserById(id);
        }


        public Task<UserRoomDto?> GetUserRoomsById(int id)
        {
            return _repository.GetUserRoomsById(id);
        }
    }
}