using System.Text.Json.Serialization;

namespace ApiPersonas.Modelos.DtoExternos
{
    public class AutorDto
    {
        [JsonPropertyName("id")]
        public int Id{ get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
    }
}
