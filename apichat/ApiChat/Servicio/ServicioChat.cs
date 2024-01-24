using ApiChat.Modelos.Dto;
using ApiChat.Modelos.DtoExternos;
using ApiLibros.Modelos.Dto;
using ApiLibros.Servicio.IServicio;
using RestSharp;
using System.Text.Json;

namespace ApiLibros.Servicio
{
    public class ServicioChat : IServicioChat
    {
        private readonly string _routeApiLibros;
        private readonly string _routeApiPersonas;
        private readonly string _routeApiAutores;
        private readonly string _routeApiEditoriales;

        public ServicioChat(string routeApiLibros, string routeApiPersonas, string routeApiAutores, string routeApiEditoriales)
        {
            _routeApiLibros = routeApiLibros;
            _routeApiPersonas = routeApiPersonas;
            _routeApiAutores = routeApiAutores;
            _routeApiEditoriales = routeApiEditoriales;
        }
        public ResultDto<LibroDto> RecuperarLibroPorId(int idLibro)
        {
            var request = new RestRequest($"/{idLibro}", Method.Get);
            return ExecuteRequest<LibroDto>(_routeApiLibros, $"/{idLibro}", request);
        }
        public ResultDto<PersonasDto> RecuperarPersonaPorId(int idPersona)
        {
            var request = new RestRequest($"/{idPersona}", Method.Get);
            return ExecuteRequest<PersonasDto>(_routeApiPersonas, $"/{idPersona}", request);
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
                result.ResponseInfo = new   ResponseDto
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
