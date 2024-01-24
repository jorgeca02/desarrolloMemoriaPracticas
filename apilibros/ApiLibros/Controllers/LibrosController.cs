using ApiLibros.Modelos.Dto;
using ApiLibros.Modelos;
//using ApiLibros.Modelos.Dto;
using ApiLibros.Repositorio.IRepositorio;
//using ApiLibros.Servicio.IServicio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using ApiLibros.Repositorio;
using ApiLibros.Servicio.IServicio;
using ApiLibros.Modelos.DtoExternos;

namespace ApiLibros.Controllers
{
    [Route("libros")]
    [ApiController]
    [Tags("Libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibrosRepositorio _librosRepo;
        private readonly IMapper _mapper;
        private readonly IServicioLibros _servicioLibros;
        
        public LibrosController(IServicioLibros servicioLibros, ILibrosRepositorio librosRepo,IMapper mapper)
        {
            _servicioLibros = servicioLibros;
            _librosRepo = librosRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LibroDto>))]
        [SwaggerOperation(Summary = "Obtiene la lista de libros.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(List<LibroDto>))]
        public IActionResult GetListaLibros([FromQuery]int autor,int editorial, int idPersona, string titulo, string ciudad, int codigoPostal)
        {
            var responseContent = _librosRepo.GetListaLibros(autor, editorial,idPersona, titulo);
            foreach(LibroDto libroDto in responseContent)
            {
                libroDto.Autor = new AutorDto();
                libroDto.Editorial = new EditorialDto();
                libroDto.Autor = _servicioLibros.RecuperarAutorPorId(libroDto.autorId).Data;
                libroDto.Editorial = _servicioLibros.RecuperarEditorialPorId(libroDto.EditorialId).Data;
            }
            if(ciudad != null || codigoPostal != 0)
            {
                var response2 = _servicioLibros.FiltrarLibros(ciudad, codigoPostal,responseContent);
                if (response2.StatusCode == StatusCodes.Status200OK)
                {
                    responseContent = response2.Data;
                }
                else
                {
                    return BadRequest(response2);
                }
            }
            return Ok(responseContent);
        }

        [HttpGet("{idLibro}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibroDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Obtiene la dirección por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(LibroDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No se encontró la dirección.")]
        public IActionResult GetLibrPorId(int idLibro)
        {
            var libroDto = _librosRepo.GetLibroPorId(idLibro);
            libroDto.Autor= new AutorDto();    
            libroDto.Editorial= new EditorialDto();
            libroDto.Autor = _servicioLibros.RecuperarAutorPorId(libroDto.autorId).Data;
            libroDto.Editorial = _servicioLibros.RecuperarEditorialPorId(libroDto.EditorialId).Data;
            return Ok(libroDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Crea un Libro.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error servidor", typeof(ResponseDto))]
        public IActionResult CrearLibro([FromBody] LibroDto libroDto)
        {
            var resp=_librosRepo.AddLibro(libroDto);
            return Ok(resp);
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
        public IActionResult ActualizarLibro([FromBody] LibroDto libroDto, int idUsuario)
        {
            _librosRepo.PutLibro(libroDto);
            return Ok();
        }


        [HttpDelete("{idLibro}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Elimina un Dirección.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        public IActionResult EliminarDireccion(int idLibro)
        {
            _servicioLibros.EliminarChats(idLibro);
            _librosRepo.DeleteLibro(idLibro);
            return Ok();
        }
    }
}
