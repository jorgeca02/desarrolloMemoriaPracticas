using ApiLibros.Modelos.DtoExternos;
using System.Reflection;

namespace ApiLibros.Modelos.Dto
{
    public class LibroDto
    {
        public int Id { get; set; }
        public String Titulo { get; set; }
        public String Descripcion { get; set; }
        public int autorId { get; set; }
        public AutorDto Autor { get; set; }
        public int EditorialId { get; set; }
        public EditorialDto Editorial { get; set; }
        public int IdPersona { get; set; }
        public int Free { get; set; }
    }
}
