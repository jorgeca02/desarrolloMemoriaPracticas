using ApiLibros.Modelos;
using ApiLibros.Modelos.Dto;

namespace ApiLibros.Repositorio.IRepositorio
{
    public interface ILibrosRepositorio
    {
        List<LibroDto> GetListaLibros(int autor, int editorial, int persona, string titulo);
        LibroDto GetLibroPorId(int id);
        ResponseDto AddLibro(LibroDto libro);
        int PutLibro(LibroDto libro);
        int DeleteLibro(int id);
    }

}
