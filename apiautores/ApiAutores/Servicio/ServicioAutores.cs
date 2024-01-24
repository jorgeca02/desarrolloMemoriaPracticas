using ApiAutor.Modelos.Dto;
using ApiLibros.Modelos.Dto;
using ApiLibros.Servicio.IServicio;
using RestSharp;
using System.Text.Json;

namespace ApiDireccion.Servicio
{
    public class ServicioLibros : IServicioLibros
    {
        private readonly string _routeApiUsuarios;

        public ServicioLibros(string routeApiUsuarios)
        {
            _routeApiUsuarios = routeApiUsuarios;
        }

        public ResultDto<AutorDto> RecuperarUsuarioPorId(int idUsuario)
        {
            var request = new RestRequest($"/{idUsuario}", Method.Get);
            return ExecuteRequest<AutorDto>(_routeApiUsuarios, $"/{idUsuario}", request);
        }

        //public ResultDto<DireccionValidarDto> GetValidarDireccionBDC(string nomPais, string nomProvincia, string nomPueblo, string nomClase, string nomVial, string nomApp, int numApp, string escalera, string planta, string puerta, string aplicacion)
        //{
        //    var request = new RestRequest("getValidarDireccion", Method.Get);

        //    request.AddQueryParameter("nomPais", nomPais);
        //    request.AddQueryParameter("nomProvincia", nomProvincia);
        //    request.AddQueryParameter("nomPueblo", nomPueblo);
        //    request.AddQueryParameter("nomClase", nomClase);
        //    request.AddQueryParameter("nomVial", nomVial);
        //    request.AddQueryParameter("nomApp", nomApp);
        //    request.AddQueryParameter("numApp", numApp);
        //    request.AddQueryParameter("escalera", escalera);
        //    request.AddQueryParameter("planta", planta);
        //    request.AddQueryParameter("puerta", puerta);
        //    request.AddQueryParameter("aplicacion", aplicacion);

        //    return ExecuteRequest<DireccionValidarDto>(_routeApiDireccion, $"getValidarDireccion", request);
        //}

        private ResultDto<T> ExecuteRequest<T>(string baseUrl, string resource, RestRequest recRequest) where T : new()
        {
            var client = new RestClient(baseUrl);
            var request = recRequest;
            var result = new ResultDto<T>();

            request.Timeout = 10000;


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
