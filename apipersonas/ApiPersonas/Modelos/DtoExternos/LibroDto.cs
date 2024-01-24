using ApiPersonas.Modelos.DtoExternos;

namespace ApiPersonas.Modelos.DtoExternos
{
    public class LibroDto
    {
        public int id { get; set; }
        public String titulo { get; set; }
        public String descripcion { get; set; }
        public int autorId { get; set; }
        public AutorDto autor { get; set; }
        public int editorialId { get; set; }
        public EditorialDto editorial { get; set; }
        public int idPersona { get; set; }
        public int free { get; set; }
    }
}
