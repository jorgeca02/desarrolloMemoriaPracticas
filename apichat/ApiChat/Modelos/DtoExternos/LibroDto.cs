namespace ApiChat.Modelos.DtoExternos
{
    public class LibroDto
    {
        public int id { get; set; }
        public String titulo { get; set; }
        public String descripcion { get; set; }
        public int autorId { get; set; }
        public CampoDto autor { get; set; }
        public int editorialId { get; set; }
        public CampoDto editorial { get; set; }
        public int idPersona { get; set; }
        public int free { get; set; }
    }
}
