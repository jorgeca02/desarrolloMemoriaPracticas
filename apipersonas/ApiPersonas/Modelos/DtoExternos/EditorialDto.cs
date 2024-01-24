using System.Text.Json.Serialization;

namespace ApiPersonas.Modelos.DtoExternos
{
    public class EditorialDto
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("nombre")]
        public string nombre { get; set; }
    }
}
