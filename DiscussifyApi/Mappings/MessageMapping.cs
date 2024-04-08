using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Utils;
namespace DiscussifyApi.Mappings
{
    public class MessageMapping : Profile
    {
        public MessageMapping()
        {
            CreateMap<MessageCreationDto, Message>()
                .ForMember(dto => dto.DateTimeCreated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()))
                .ForMember(dto => dto.DateTimeUpdated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()));

            CreateMap<MessageUpdationDto, Message>()
                .ForMember(dto => dto.DateTimeUpdated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()));
        }
    }
}
