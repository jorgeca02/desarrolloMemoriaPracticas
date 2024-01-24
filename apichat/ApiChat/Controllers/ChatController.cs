using ApiLibros.Modelos.Dto;
//using ApiLibros.Modelos.Dto;
using ApiLibros.Repositorio.IRepositorio;
//using ApiLibros.Servicio.IServicio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ApiLibros.Servicio.IServicio;
using ApiChat.Modelos.Dto;

namespace ApiChat.Controllers
{
    [Route("chat")]
    [ApiController]
    [Tags("Chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepositorio _chatRepo;
        private readonly IMapper _mapper;
        private readonly IServicioChat _servicioChat;
        
        public ChatController(IServicioChat servicioChat, IChatRepositorio chatRepo,IMapper mapper)
        {
            _servicioChat = servicioChat;
            _chatRepo = chatRepo;
            _mapper = mapper;
        }

        [HttpGet("{idChat}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Obtiene un chat por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ChatDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No se encontró el chat")]
        public IActionResult GetChatPorId(int idChat)
        {
            var chatDto = _chatRepo.GetChatPorId(idChat);
            chatDto.libro = _servicioChat.RecuperarLibroPorId(chatDto.idLibro).Data;
            chatDto.persona1 = _servicioChat.RecuperarPersonaPorId(chatDto.idPersona).Data;
            chatDto.persona2 = _servicioChat.RecuperarPersonaPorId(chatDto.libro.idPersona).Data;
            return Ok(chatDto);

        }
        [HttpGet("/mensaje/{idChat}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Obtiene un chat por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ChatDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No se encontró el chat")]
        public IActionResult GetMensajeChatPorId(int idChat)
        {
            var ListaMensajeDto = _chatRepo.GetMensajeChatPorId(idChat);
            return Ok(ListaMensajeDto);

        }
        [HttpGet("/persona/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Obtiene un chat por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ChatDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No se encontró el chat")]
        public IActionResult GetChatPorPersona(int id)
        {

            var persona = _servicioChat.RecuperarPersonaPorId(id).Data;
            if (persona == null)
            {
                return NotFound();
            }
            var chatsDto = _chatRepo.GetChatPorIdPersona(id,persona);
            foreach (var chatDto in chatsDto)
            {
                chatDto.libro = _servicioChat.RecuperarLibroPorId(chatDto.idLibro).Data;
                if(chatDto.idPersona == id)
                {
                    chatDto.persona1 = _servicioChat.RecuperarPersonaPorId(chatDto.idPersona).Data;
                    chatDto.persona2 = _servicioChat.RecuperarPersonaPorId(chatDto.libro.idPersona).Data;
                }
                else
                {
                    chatDto.persona2 = _servicioChat.RecuperarPersonaPorId(chatDto.idPersona).Data;
                    chatDto.persona1 = _servicioChat.RecuperarPersonaPorId(chatDto.libro.idPersona).Data;
                }
            }
            return Ok(chatsDto);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Crea un Chat.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error servidor", typeof(ResponseDto))]
        public IActionResult CrearChat(int idUsuario,int idLibro)
        {   
            _chatRepo.AddChat(idUsuario,idLibro);
            return Ok();
        }
        [HttpPost("/addMensaje")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Crea un Chat.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No encontrado", typeof(ResponseDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error servidor", typeof(ResponseDto))]
        public IActionResult CrearChat(int idUsuario, int idChat, string mensaje)
        {
            _chatRepo.AddMensaje(idChat,idUsuario, mensaje);
            return Ok();
        }
        [HttpDelete("libro/{idLibro}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto))]
        [SwaggerOperation(Summary = "Elimina los chats de un libro.")]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponseDto))]
        public IActionResult EliminarLibro(int idLibro)
        {
            var resp=_chatRepo.DeletePorLibro(idLibro);
            if (resp.CodigoOperacion == 200)
            {
                return Ok(resp);
            }
            return BadRequest(resp);
            
        }
    }
}
