

using ApiChat.Modelos.DtoExternos;
using ApiLibros.Modelos.Dto;

namespace ApiLibros.Servicio.IServicio
{
    public interface IServicioChat
    {
        ResultDto<LibroDto> RecuperarLibroPorId(int idLibro);
        ResultDto<PersonasDto> RecuperarPersonaPorId(int idPersona);
    }
}
