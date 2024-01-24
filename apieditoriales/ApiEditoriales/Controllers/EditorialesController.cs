using ApiLibros.Modelos.Dto;
//using ApiLibros.Modelos.Dto;
//using ApiLibros.Servicio.IServicio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ApiEditoriales.Modelos.Dto;
using ApiEditoriales.Repositorio.IRepositorio;

namespace ApEditorales.Controllers
{
    [Route("editoriales")]
    [ApiController]
    [Tags("Editoriales")]
    public class EditorialesController : ControllerBase
    {
        private readonly IEditorialesRepositorio _editorialesRepo;
        private readonly IMapper _mapper;
        //private readonly IServicioLibros _servicioDireccion;
        
        public EditorialesController(/*IServicioLibros servicioDireccion, */IEditorialesRepositorio editorialesRepo,IMapper mapper)
        {
           // _servicioLibros = servicioLibros;
            _editorialesRepo = editorialesRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EditorialDto>))]
        [SwaggerOperation(Summary = "Obtiene la lista de editoriales.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(List<EditorialDto>))]
        public IActionResult GetListaEditoriales()
        {
            var responseContent = _editorialesRepo.GetListaEditorial();
            return Ok(responseContent);
        }

        [HttpGet("{idEditorial}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditorialDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Obtiene la editorial por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(EditorialDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No se encontró la editorial.")]
        public IActionResult GetEditorialPorId(int idEditorial)
        {
            return Ok(_editorialesRepo.GetEditorialPorId(idEditorial));
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Crea una editorial.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error servidor", typeof(ResponseDto))]
        public IActionResult CrearLibro([FromBody] EditorialDto editorialDto, int idUsuario)
        {
            _editorialesRepo.AddEditorial(editorialDto);
            return Ok();
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Actualiza un libro.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrada", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error servidor", typeof(ResponseDto))]
        public IActionResult ActualizarEditorial([FromBody] EditorialDto editorialDto, int idUsuario)
        {
            
            var respuesta = _editorialesRepo.AddEditorial(editorialDto);
            if(respuesta== 404)
            {
                return NotFound();
            }
            if(respuesta == 401)
            {
                return BadRequest(new ResponseDto
                {
                    Mensaje = "editorial ya existe",
                });
                
            }

            return Ok();
        }


        [HttpDelete("{idEditorial}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Elimina una editorial.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        public IActionResult EliminarEditorial(int idEditorial)
        {
            _editorialesRepo.DeleteEditorial(idEditorial);
            return Ok();
        }
    }
}
