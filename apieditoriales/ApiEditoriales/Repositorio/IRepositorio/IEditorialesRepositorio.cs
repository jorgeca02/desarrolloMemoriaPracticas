using ApiEditoriales.Modelos.Dto;

namespace ApiEditoriales.Repositorio.IRepositorio
{
    public interface IEditorialesRepositorio
    {
        List<EditorialDto> GetListaEditorial();
        EditorialDto GetEditorialPorId(int id);
        int AddEditorial(EditorialDto libro);
        int PutEditorial(EditorialDto libro);
        int DeleteEditorial(int id);
    }

}
