using AutoMapper;
using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Modelos
{
	public class MappingConfig : Profile
	{
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpDateDto>().ReverseMap();
        }
    }
}
