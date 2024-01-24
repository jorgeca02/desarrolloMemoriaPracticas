using ApiAutor.Modelos;
using ApiAutor.Modelos.Dto;
using AutoMapper;

namespace ApiAutores.Mapper
{
    public class AutorMapper : Profile
    {
        public AutorMapper()
        {
            CreateMap<Autores, AutorDto>()
                .ReverseMap();
        }
    }
}
