using ApiChat.Modelos.DtoExternos;

namespace ApiChat.Modelos.Dto
{
    public class ChatDto
    {
        public int id { get; set; }
        public int idPersona { get; set; }
        public int idLibro { get; set; }
        public PersonasDto persona1 { get; set; }
        public PersonasDto persona2 { get; set; }
        public LibroDto libro { get; set; }
        public List<MensajeDto> mensajes { get; set; }
    }
}
