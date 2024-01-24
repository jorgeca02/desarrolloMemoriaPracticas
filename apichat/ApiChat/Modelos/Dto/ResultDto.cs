using ApiChat.Modelos.Dto;
using System.Text.Json.Serialization;

namespace ApiLibros.Modelos.Dto
{
    public class ResultDto<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("responseInfo")]
        public ResponseDto ResponseInfo { get; set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }
    }

}