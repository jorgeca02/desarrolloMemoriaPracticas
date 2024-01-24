using ApiPersonas.Modelos;
using ApiPersonas.Modelos.Dto;
using AutoMapper;

namespace ApiPersonas.Mapper
{
    public class PersonasMapper : Profile
    {
        public PersonasMapper()
        {
            CreateMap<Personas, PersonasDto>()
                .ReverseMap();

        }
                              
    }
}
