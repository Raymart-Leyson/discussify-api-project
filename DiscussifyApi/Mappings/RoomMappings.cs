using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Utils;

namespace DiscussifyApi.Mappings
{
    public class RoomMappings : Profile
    {
        public RoomMappings()
        {
            CreateMap<RoomCreationDto, Room>()
                .ForMember(dto => dto.DateTimeCreated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()))
                .ForMember(dto => dto.DateTimeUpdated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()));

            CreateMap<RoomUpdationDto, Room>()
                .ForMember(dto => dto.DateTimeUpdated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()));
        }
    }
}
