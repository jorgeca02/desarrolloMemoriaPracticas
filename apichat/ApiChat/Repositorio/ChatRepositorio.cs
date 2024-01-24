using ApiChat.Modelos.Dto;
using ApiChat.Modelos.DtoExternos;
using ApiLibros.Data;
using ApiLibros.Modelos;
using ApiLibros.Modelos.Dto;
using ApiLibros.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Transactions;

namespace ApiLibros.Repositorio
{
    public class ChatRepositorio : IChatRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;
        public ChatRepositorio(ApplicationDbContext bd, IMapper mapper) 
        {
            _bd = bd;
            _mapper = mapper;
        }
        public ChatDto GetChatPorId(int idChat)
        {
            var chat = _bd.Chat.FirstOrDefault(x => x.Id == idChat);
            var chatDto = new ChatDto();
            _mapper.Map(chat, chatDto); 
            chatDto.mensajes = _mapper.Map<List<MensajeDto>>(_bd.Mensaje.Where(x => x.IdChat == idChat).ToList());
            return chatDto;
        }
        public List<MensajeDto> GetMensajeChatPorId(int idChat)
        {
            var ListaMensaje = _bd.Mensaje.Where(x => x.IdChat == idChat).ToList();
            var ListaMensajeDto = new List<MensajeDto>();
            _mapper.Map(ListaMensaje, ListaMensajeDto);
            return ListaMensajeDto;
        }
        public List<ChatDto> GetChatPorIdPersona(int id, PersonasDto persona)
        {
            var chats = _bd.Chat.ToList();
            chats = chats
                .Where(x => x.IdPersona == id ||
                (persona != null && persona.libros.Any(libro => libro.id == x.IdLibro)))
                .ToList();
            var chatsDto = new List<ChatDto>();
            foreach (var chat in chats)
            {
                var chatDto = new ChatDto();
                _mapper.Map(chat, chatDto);
                chatDto.mensajes = _mapper.Map<List<MensajeDto>>(_bd.Mensaje.Where(x => x.IdChat == chat.Id).ToList());
                chatsDto.Add(chatDto);
            }
            return chatsDto;
        }
        public ResponseDto AddChat(int id1, int id2)
        {
            using var transaction = _bd.Database.BeginTransaction();
            var chat = new Chat();
            chat.IdPersona = id1;
            chat.IdLibro = id2;
            _bd.Chat.Add(chat);
            if (Guardar())
            {
                transaction.Commit();
                return new ResponseDto
                {
                    CodigoOperacion = 200
                };
            }
            transaction.Rollback();
            return new ResponseDto
            {
                CodigoOperacion = 400
            };
            
        }

        public ResponseDto AddMensaje(int idChat, int idPersona, string mensaje)
        {
            using var transaction = _bd.Database.BeginTransaction();
            var mensajeDto = new MensajeDto()
            {
                id = 0,
                idChat = idChat,
                idPersona = idPersona,
                texto = mensaje
            };
            var mensajeDb = new Mensaje();

            _bd.Mensaje.Add(_mapper.Map(mensajeDto,mensajeDb));
            if (Guardar())
            {
                transaction.Commit();
                return new ResponseDto
                {
                    CodigoOperacion = 200
                };
            }
            transaction.Rollback();
            return new ResponseDto
            {
                CodigoOperacion = 400
            };

        }

        public ResponseDto DeletePorLibro(int idLibro)
        {
            using var transaction = _bd.Database.BeginTransaction();
            var chats = _bd.Chat.Where(x => x.IdLibro == idLibro).ToList();
            foreach (var chat in chats)
            {
                _bd.Chat.Remove(chat);
            }
            if (Guardar())
            {
                transaction.Commit();
                return new ResponseDto
                {
                    CodigoOperacion = 200
                };
            }
            transaction.Rollback();
            return new ResponseDto
            {
                CodigoOperacion = 400
            };
        }
        public bool Guardar()
        {
            return _bd.SaveChanges() > 0;
        }

        
    }

}
