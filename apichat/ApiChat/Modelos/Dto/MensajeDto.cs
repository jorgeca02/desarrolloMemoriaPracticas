namespace ApiChat.Modelos.Dto
{
    public class MensajeDto
    {
        public int id { get; set; }
        public int idChat { get; set; }
        public int idPersona { get; set; }
        public string texto { get; set; }
    }
}
