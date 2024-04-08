using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Repositories;
using DiscussifyApi.Utils;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace DiscussifyApi.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Room> CreateRoom(RoomCreationDto room)
        {
            var model = _mapper.Map<Room>(room);
            var lastRoomId = await _repository.GetLastRoomId();
            lastRoomId = lastRoomId < 2 ? 1 : lastRoomId + 1;
            model.Code = $"{StringUtil.RandomString(6)}-{lastRoomId}"; 
            model.Id = await _repository.CreateRoom(model);
            return model;
        }

        public Task<IEnumerable<RoomUserDto>> GetAllRooms()
        {
            return _repository.GetAllRooms();
        }

        public Task<int> GetRoomIdByCode(string code)
        {
            return _repository.GetRoomIdByCode(code);
        }

        public Task<RoomUserDto?> GetRoomById(int id)
        {
            return _repository.GetRoomById(id);
        }

        public async Task<int> UpdateRoom(int id, RoomUpdationDto room)
        {
            var model = _mapper.Map<Room>(room);
            model.Id = id;

            return await _repository.UpdateRoom(model);
        }

        public async Task<int> DeleteRoom(int id)
        {
            return await _repository.DeleteRoom(id);
        }
    }
}

