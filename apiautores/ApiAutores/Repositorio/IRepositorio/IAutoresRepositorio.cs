using ApiAutor.Modelos.Dto;
using ApiLibros.Modelos.Dto;

namespace ApiAutores.Repositorio.IRepositorio
{
    public interface IAutoresRepositorio
    {
        List<AutorDto> GetListaAutores();
        AutorDto GetAutorPorId(int id);
        int AddAutor(AutorDto autorDto);
        int PutAutor(AutorDto autorDto);
        int DeleteAutor(int id);
    }

}
