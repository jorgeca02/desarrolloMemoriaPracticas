using System.Text.Json.Serialization;

namespace ApiPersonas.Modelos.Dto
{
    public class ResponseDto
    {
        [JsonPropertyName("codigoOperacion")]
        public int CodigoOperacion { get; set; }
        [JsonPropertyName("descripcionOperacion")]
        public string DescripcionOperacion { get; set; }
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }
        [JsonPropertyName("idObjeto")]
        public string IdObjeto { get; set; }

    }
}
