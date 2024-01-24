using ApiPersonas.Modelos.Dto;
using ApiPersonas.Repositorio.IRepositorio;
using ApiPersonas.Servicio.IServicio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiPersonas.Controllers
{
    [Route("personas")]
    [ApiController]
    [Tags("Personas")]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonasRepositorio _personasRepo;
        private readonly IMapper _mapper;
        private readonly IServicioPersonas _servicioPersonas;

        public PersonasController(IPersonasRepositorio personasRepo, IMapper mapper, IServicioPersonas servicioPersonas)
        {
            _personasRepo = personasRepo;
            _mapper = mapper;
            _servicioPersonas = servicioPersonas;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PersonasDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Obtiene lista de personas (Solo obtiene informacion Persona)")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(List<PersonasDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno servidor", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status502BadGateway, "Bad Gateway", typeof(ResponseDto))]
        public IActionResult GetListaPersonas(bool libros = true)
        {
            var listaPersona = _personasRepo.GetPersonas();
            var listaPersonaDto = _mapper.Map<List<PersonasDto>>(listaPersona);
            if(libros)
            {
                foreach (var personasDto in listaPersonaDto)
                {
                    personasDto.libros = _servicioPersonas.RecuperarLibrosPorPersonaId(personasDto.Id).Data;
                }
            }
            return Ok(listaPersonaDto);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonasDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Obtiene persona por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(PersonasDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno servidor", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status502BadGateway, "Bad Gateway", typeof(ResponseDto))]
        public IActionResult GetPersona(int id)
        {
            var personaDto = _personasRepo.GetPersonaPorId(id);
            if (personaDto == null)
            {
                return NotFound(new ResponseDto
                {
                    CodigoOperacion = 404,
                    DescripcionOperacion = "Persona inexsistente",
                    Mensaje = $"Persona con id {id} no encontrado: no existe o esta dado de baja"
                });
            }
            personaDto.libros = _servicioPersonas.RecuperarLibrosPorPersonaId(personaDto.Id).Data;
            return Ok(personaDto);
        }
        [HttpGet("/login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonasDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Obtiene persona por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(PersonasDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "usuario no encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno servidor", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status502BadGateway, "Bad Gateway", typeof(ResponseDto))]
        public IActionResult login([FromQuery]string username, string password)
        {
            if (username == null || password == null || password == "" || username == "")
            {
                return BadRequest(new ResponseDto
                {
                    CodigoOperacion = 400,
                    DescripcionOperacion = "Solicitud incorrecta",
                    Mensaje = "Los parametros username y password no pueden estar vacios"
                });
            }
            var respuesta = _personasRepo.Login(username,password);
            if (respuesta.CodigoOperacion == 200)
                return Ok(respuesta);
            else 
                return BadRequest(respuesta);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Crear una persona")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status502BadGateway, "Bad Gateway", typeof(ResponseDto))]
        public IActionResult CrearPersona(PersonasDto personaDto)
        {
            if (personaDto == null)
            {
                return BadRequest(new ResponseDto
                {
                    CodigoOperacion = 400,
                    DescripcionOperacion = "Solicitud incorrecta",
                    Mensaje = "El parametro de entrada no puede ser vacía"
                });
            }

            if (personaDto.username==null || personaDto.password==null ||personaDto.password==""||personaDto.username=="")
            {
                return BadRequest(new ResponseDto
                {
                    CodigoOperacion = 400,
                    DescripcionOperacion = "Solicitud incorrecta",
                    Mensaje = "Los parametros username y password no pueden estar vacios"
                });
            }
            if(personaDto.username.Contains(" "))
            {
                return BadRequest(new ResponseDto
                {
                    CodigoOperacion = 400,
                    DescripcionOperacion = "Solicitud incorrecta",
                    Mensaje = "El username no puede contener espacios"
                });
            }
            var respuestaCrear = _personasRepo.AddPersona(personaDto);
            if (respuestaCrear.CodigoOperacion == 200)
            {
                return Ok(respuestaCrear);
            }
            return BadRequest(respuestaCrear);
            
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Actualizar una persona")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status502BadGateway, "Bad Gateway", typeof(ResponseDto))]
        public IActionResult ActualizarPersona(PersonasDto personaDto, int idUsuarioModifica)
        {
            if (personaDto == null)
            {
                return BadRequest(new ResponseDto
                {
                    CodigoOperacion = 400,
                    DescripcionOperacion = "Solicitud incorrecta",
                    Mensaje = "El parametro de entrada no puede ser vacía"
                });
            }


            var respuestaActualizar = _personasRepo.PutPersona(personaDto);
            return Ok(respuestaActualizar);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Borrar relaciones actuacion-enCalidadDe de una persona")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(ResponseDto))]
        public IActionResult DeletePersona(int idPersona)
        {
            if (idPersona == 0)
            {
                return BadRequest(new ResponseDto
                {
                    CodigoOperacion = 400,
                    DescripcionOperacion = "Solicitud incorrecta",
                    Mensaje = "El parametro idPersona es obligatorio"
                });
            }
            var respuestaBaja = _personasRepo.DeletePersona(idPersona);
            return Ok(respuestaBaja);
        }
    }
}
