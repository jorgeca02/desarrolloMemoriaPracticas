using ApiLibros.Modelos.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ApiAutor.Modelos.Dto;
using ApiAutores.Repositorio.IRepositorio;

namespace ApiAutores.Controllers
{
    [Route("Autores")]
    [ApiController]
    [Tags("Libros")]
    public class AutoresController : ControllerBase
    {
        private readonly IAutoresRepositorio _autoresRepo;
        private readonly IMapper _mapper;
        //private readonly IServicioLibros _servicioDireccion;
        
        public AutoresController(/*IServicioLibros servicioDireccion, */IAutoresRepositorio autoresRepo,IMapper mapper)
        {
           // _servicioLibros = servicioLibros;
           _autoresRepo = autoresRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AutorDto>))]
        [SwaggerOperation(Summary = "Obtiene la lista de autores.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(List<AutorDto>))]
        public IActionResult GetListaAutores()
        {
            var responseContent = _autoresRepo.GetListaAutores();
            return Ok(responseContent);
        }

        [HttpGet("{idAutor}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AutorDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Obtiene un autor por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(AutorDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No se encontró la dirección.")]
        public IActionResult GetAutorPorId(int idAutor)
        {
            return Ok(_autoresRepo.GetAutorPorId(idAutor));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Crea un Autor.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error servidor", typeof(ResponseDto))]
        public IActionResult CrearAutor([FromBody] AutorDto autoresDto)
        {
            _autoresRepo.AddAutor(autoresDto);
            return Ok();
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Actualiza un autor.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrada", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error servidor", typeof(ResponseDto))]
        public IActionResult ActualizarAutor([FromBody] AutorDto autoresDto)
        {
            _autoresRepo.PutAutor(autoresDto);
            return Ok();
        }


        [HttpDelete("{idAutores}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Elimina un autor.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        public IActionResult EliminarAutor(int idAutor)
        {
            _autoresRepo.DeleteAutor(idAutor);
            return Ok();
        }
    }
}
