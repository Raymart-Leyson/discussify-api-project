using AutoMapper;
using DiscussifyApi.Dtos;
using DiscussifyApi.Models;
using DiscussifyApi.Utils;

namespace DiscussifyApi.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<UserCreationDto, User>()
                .ForMember(dto => dto.Password, opt => opt.MapFrom(st => Crypto.Hash(st.Password!)))
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.FirstName!)))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.LastName!)))
                .ForMember(dto => dto.DateTimeCreated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()));

            CreateMap<UserUpdationDto, User>()
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.FirstName!)))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.LastName!)));

            CreateMap<UserAuthDto, User>()
                .ForMember(dto => dto.EmailAddress, opt => opt.MapFrom(st => st.EmailAddress!))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(st => st.Password!));
        }
    }
}

