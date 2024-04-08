using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Utils;

namespace DiscussifyApi.Mappings
{
    public class AnonymousMapping : Profile
    {
        public AnonymousMapping()
        {
            CreateMap<AnonymousCreationDto, Anonymous>()
                .ForMember(dto => dto.DateTimeCreated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()));
        }
    }
}