using AutoMapper;
using Business.TransferObjects;
using Data.Models;

namespace Business.Mappings
{
    public class ExemploMapper : Profile
    {
        public ExemploMapper()
        {
            CreateMap<Exemplo, ExemploDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
