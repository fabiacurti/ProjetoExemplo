using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class ExemploSubItemMapper : Profile
    {
        public ExemploSubItemMapper()
        {
            CreateMap<ExemploSubItem, ExemploSubItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
