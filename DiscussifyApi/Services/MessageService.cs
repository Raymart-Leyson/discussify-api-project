using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Repositories;

namespace DiscussifyApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MessageGetDto> CreateMessage(int anonymousId, MessageCreationDto message)
        {
            var model = _mapper.Map<Message>(message);
            model.AnonymousId = anonymousId;
            model.Id = await _repository.CreateMessage(model);

            var newMessage = await _repository.GetMessageById(model.Id);

            return newMessage;
        }

        public async Task<IEnumerable<MessageGetDto>> GetAllMessages(int id)
        {
            return await _repository.GetAllMessages(id);
        }

        public async Task<MessageGetDto> GetMessageById(int id)
        {
            return await _repository.GetMessageById(id);
        }

        public async Task<MessageGetDto> UpdateMessage(int anonymousId, int id, MessageUpdationDto message)
        {
            var model = _mapper.Map<Message>(message);
            model.AnonymousId = anonymousId;
            model.Id = id;

            await _repository.UpdateMessage(model);

            var updatedMessage = await _repository.GetMessageById(id);

            return updatedMessage;
        }

        public async Task<MessageGetDto> UpvoteMessage(int id, int increment)
        {
            return await _repository.UpvoteMessage(id, increment);
        }

        public async Task<MessageGetDto> DownvoteMessage(int id, int decrement)
        {
            return await _repository.DownvoteMessage(id, decrement);
        }

        public async Task<int> DeleteMessage(int id)
        {
            return await _repository.DeleteMessage(id);
        }
    }
}
