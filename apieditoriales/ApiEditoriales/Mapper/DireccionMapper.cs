using ApiEditoriales.Modelos.Dto;
using ApiLibros.Modelos;
using ApiLibros.Modelos.Dto;
using AutoMapper;

namespace ApiLibros.Mapper
{
    public class DireccionMapper : Profile
    {
        public DireccionMapper()
        {
            CreateMap<Editoriales, EditorialDto>()
                .ReverseMap();
        }
    }
}
