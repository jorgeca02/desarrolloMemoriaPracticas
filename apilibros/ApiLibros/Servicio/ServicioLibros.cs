using ApiLibros.Modelos.Dto;
using ApiLibros.Modelos.DtoExternos;
using ApiLibros.Servicio.IServicio;
using RestSharp;
using System.Text.Json;

namespace ApiLibros.Servicio
{
    public class ServicioLibros : IServicioLibros
    {
        private readonly string _routeApiAutores;
        private readonly string _routeApiEditoriales;
        private readonly string _routeApiPersonas;
        private readonly string _routeApiChats;

        public ServicioLibros(string routeApiAutores,string routeApiEditoriales, string routeApiPersonas, string routeApiChats)
        {
            _routeApiAutores = routeApiAutores;
            _routeApiEditoriales = routeApiEditoriales;
            _routeApiPersonas = routeApiPersonas;
            _routeApiChats = routeApiChats;
        }

        public ResultDto<AutorDto> RecuperarAutorPorId(int idAutor)
        {
            var request = new RestRequest($"/{idAutor}", Method.Get);
            return ExecuteRequest<AutorDto>(_routeApiAutores, $"/{idAutor}", request);
        }
        public ResultDto<EditorialDto> RecuperarEditorialPorId(int idEditorial)
        {
            var request = new RestRequest($"/{idEditorial}", Method.Get);
            return ExecuteRequest<EditorialDto>(_routeApiEditoriales, $"/{idEditorial}", request);
        }
        
        public ResultDto<List<LibroDto>> FiltrarLibros(string ciudad, int codigoPostal,List<LibroDto> listaPre)
        {
            var request = new RestRequest("?libros=false", Method.Get);
            var result = ExecuteRequest<List<PersonasDto>>(_routeApiPersonas, "?libros=false", request);
            if (result.IsSuccess==false)
            {
                return new ResultDto<List<LibroDto>>
                {
                    IsSuccess = false,
                    ResponseInfo = result.ResponseInfo
                };
            }
            var lista1 = new List<LibroDto>();
            foreach (var libro in listaPre)
            {
                if (libro.IdPersona != 0)
                {
                    var persona = result.Data.Find(p => p.id == libro.IdPersona);

                    if (persona != null)
                    {
                        var boolCiu = false;
                        var boolCod = false;
                        if (persona.ciudad != null && ciudad != null)
                        {
                            boolCiu = true;
                        }
                        if (persona.codigoPostal != 0 && codigoPostal != 0)
                        {
                            boolCod = true;
                        }
                        if (boolCiu && boolCod)
                        {
                            if (persona.ciudad.ToLower().Contains(ciudad.ToLower()) && persona.codigoPostal == codigoPostal)
                            {
                                lista1.Add(libro);
                            }
                        }
                        else if (boolCiu)
                        {
                            if (persona.ciudad.ToLower().Contains(ciudad.ToLower()))
                            {
                                lista1.Add(libro);
                            }
                        }
                        else if (boolCod)
                        {
                            if (persona.codigoPostal == codigoPostal)
                            {
                                lista1.Add(libro);
                            }
                        }

                    }
                }
            }
            return new ResultDto<List<LibroDto>>()
            {
                StatusCode = 200,
                IsSuccess = true,
                Data = lista1
            };
        }
        public ResultDto<ResponseDto> EliminarChats(int idLibro)
        {
            var request = new RestRequest($"/libro/{idLibro}", Method.Delete);
            return ExecuteRequest<ResponseDto>(_routeApiChats, $"/{idLibro}", request);
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
