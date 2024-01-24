using ApiPersonas.Modelos.Dto;
using ApiPersonas.Modelos.DtoExternos;

namespace ApiPersonas.Servicio.IServicio
{
    public interface IServicioPersonas
    {
        ResultDto<LibroDto> RecuperarLibroPorId(int idLibro);
        ResultDto<List<LibroDto>> RecuperarLibrosPorPersonaId(int idPersona);
    }
}
