using ApiLibros.Modelos;
using ApiLibros.Modelos.Dto;
using AutoMapper;

namespace ApiLibros.Mapper
{
    public class LibrosMapper : Profile
    {
        public LibrosMapper()
        {
            CreateMap<Libros, LibroDto>()
                .ForMember(dest => dest.Autor, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
