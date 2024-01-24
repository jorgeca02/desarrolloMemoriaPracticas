namespace ApiChat.Modelos.DtoExternos
{
    public class PersonasDto
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set;}
        public string ciudad { get; set; }
        public int codigoPostal { get; set; }
        public List<LibroDto> libros { get; set; }
    }
}
