using ApiPersonas.Modelos.DtoExternos;

namespace ApiPersonas.Modelos.Dto
{
    public class PersonasDto
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set;}
        public List<LibroDto> libros { get; set; }

        public string ciudad { get; set; }
        public int codigoPostal { get; set; }
    }
}
