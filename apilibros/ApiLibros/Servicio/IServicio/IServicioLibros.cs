using ApiLibros.Modelos.Dto;
using ApiLibros.Modelos.DtoExternos;
using RestSharp;

namespace ApiLibros.Servicio.IServicio
{
    public interface IServicioLibros
    {
        ResultDto<AutorDto>RecuperarAutorPorId(int idAutor);
        ResultDto<EditorialDto> RecuperarEditorialPorId(int idEditorial);

        ResultDto<List<LibroDto>> FiltrarLibros(string ciudad, int codigoPostal, List<LibroDto> listaPre);
        ResultDto<ResponseDto> EliminarChats(int idLibro);
    }
}
