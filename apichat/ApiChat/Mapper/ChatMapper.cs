using ApiChat.Modelos.Dto;
using ApiLibros.Modelos;
using ApiLibros.Modelos.Dto;
using AutoMapper;

namespace ApiLibros.Mapper
{
    public class ChatMapper : Profile
    {
        public ChatMapper()
        {
            CreateMap<Chat, ChatDto>()
                .ReverseMap();
            CreateMap<Mensaje, MensajeDto>()
                .ReverseMap();
        }
    }
}
