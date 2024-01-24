using ApiPersonas.Modelos.Dto;
using ApiPersonas.Modelos.DtoExternos;
using ApiPersonas.Servicio.IServicio;
using RestSharp;
using System.Text.Json;

namespace ApiPersonas.Servicio
{
    public class ServicioPersonas : IServicioPersonas
    {
        private readonly string _routeApiLibros;

        public ServicioPersonas(string routeApiLibros)
        {
            _routeApiLibros=routeApiLibros;
        }
        public ResultDto<LibroDto> RecuperarLibroPorId(int idLibro)
        {
            var request = new RestRequest($"/{idLibro}", Method.Get);
            return ExecuteRequest<LibroDto>(_routeApiLibros, $"/{idLibro}", request);
        }
        public ResultDto<PersonasDto> RecuperarPersonaPorId(int idPersona)
        {
            var request = new RestRequest($"/{idPersona}", Method.Get);
            return ExecuteRequest<PersonasDto>(_routeApiLibros, $"/{idPersona}", request);
        }
        public ResultDto<List<LibroDto>> RecuperarLibrosPorPersonaId(int idPersona)
        {
            var request = new RestRequest($"?idPersona={idPersona}", Method.Get);
            return ExecuteRequest<List<LibroDto>>(_routeApiLibros, $"?idPersona={idPersona}", request);
        }
        private ResultDto<T> ExecuteRequest<T>(string baseUrl, string resource, RestRequest recRequest) where T : new()
        {
            var client = new RestClient(baseUrl);
            var request = recRequest;
            var result = new ResultDto<T>();

            request.Timeout = 90000;


            try
            {
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    T data = JsonSerializer.Deserialize<T>(response.Content);
                    result.Data = data;
                    result.IsSuccess = true;
                    return result;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result.ResponseInfo = new ResponseDto
                    {
                        CodigoOperacion = 404,
                        DescripcionOperacion = "Not Found",
                        Mensaje = "Recurso no encontrado",
                        IdObjeto = resource
                    };
                    result.IsSuccess = false;
                    return result;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    result.ResponseInfo = new ResponseDto
                    {
                        CodigoOperacion = 500,
                        DescripcionOperacion = "Error interno del servidor",
                        Mensaje = "Error al conectar con la API externa"
                    };
                }
                else if (response.StatusCode == 0)
                {
                    result.ResponseInfo = new ResponseDto
                    {
                        CodigoOperacion = 502,
                        DescripcionOperacion = "Bad Gateway",
                        Mensaje = "Error al conectar con la API externa"
                    };

                }
            }
            catch (Exception ex)
            {
                result.ResponseInfo = new ResponseDto
                {
                    CodigoOperacion = 500,
                    DescripcionOperacion = "Error",
                    Mensaje = ex.Message,
                    IdObjeto = null
                };
                result.IsSuccess = false;
            }
            return result;
        }
    }
}
