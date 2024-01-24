using ApiChat.Modelos.Dto;
using ApiChat.Modelos.DtoExternos;
using ApiLibros.Modelos;

namespace ApiLibros.Repositorio.IRepositorio
{
    public interface IChatRepositorio
    {
        ChatDto GetChatPorId(int id);
        List<ChatDto> GetChatPorIdPersona(int id, PersonasDto persona);
        List<MensajeDto> GetMensajeChatPorId(int idChat);
        ResponseDto AddChat(int id1,int id2);
        ResponseDto AddMensaje(int idChat,int idPersona, string mensaje);
        ResponseDto DeletePorLibro(int idLibro);
    }
}
