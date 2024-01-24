using ApiPersonas.Data;
using ApiPersonas.Modelos;
using ApiPersonas.Modelos.Dto;
using ApiPersonas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Text;
using System.Transactions;

namespace ApiPersonas.Repositorio
{
    public class PersonasRepositorio : IPersonasRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;
        public PersonasRepositorio(ApplicationDbContext bd, IMapper mapper)
        {
            _bd = bd;
            _mapper = mapper;
        }

        public List<PersonasDto> GetPersonas()
        {
            var lista = _bd.Personas.ToList();
            var listaDto = new List<PersonasDto>();
            _mapper.Map(lista, listaDto);
            
            return listaDto;
        }
        public PersonasDto GetPersonaPorId(int idPersona) 
        {
            var persona= _bd.Personas.FirstOrDefault(p => p.Id == idPersona);
            var personaDto = new PersonasDto();
            _mapper.Map(persona, personaDto);
            
            return personaDto;
        }
        public ResponseDto Login(string username, string password)
        {
            var personaExiste = _bd.Personas.FirstOrDefault(p => p.Username == username);
            if (personaExiste == null)
            {
                return new ResponseDto
                {
                    CodigoOperacion=404,
                    Mensaje="usuario no existe, si no tienes cuenta, utiliza el boton signin"
                };
            }
            if(personaExiste.Password != password)
            {
                return new ResponseDto
                {
                    CodigoOperacion = 400,
                    Mensaje = "contraseña incorrecta"
                };
            }
            return new ResponseDto
            {
                CodigoOperacion = 200,
                IdObjeto = personaExiste.Id.ToString(),
            };
        }

        public ResponseDto AddPersona(PersonasDto persona)
        {
            using var transaction = _bd.Database.BeginTransaction();
            var personaExiste=_bd.Personas.FirstOrDefault(p=>p.Username == persona.username);
            if(personaExiste != null)
            {
                transaction.Rollback();
                return new ResponseDto
                {
                    CodigoOperacion = 400,
                    Mensaje = "ya existe un usuario con ese username"
                };
            }
            var personaDb = new Personas();
            _mapper.Map(persona, personaDb);
            _bd.Personas.Add(personaDb);
            if (Guardar())
            {
                transaction.Commit();
                return new ResponseDto
                {
                    CodigoOperacion = 200,
                    IdObjeto=personaDb.Id.ToString(),
                
                };
            }
            return new ResponseDto
            {
                CodigoOperacion = 400
            }; ;

        }
        public int PutPersona(PersonasDto personaDto)
        {
            using var transaction = _bd.Database.BeginTransaction();
            var persona = _bd.Personas.FirstOrDefault(p => p.Id == personaDto.Id);
            if (persona == null)
            {
                return 404;
            }
            else
            {
                _mapper.Map(personaDto, persona);
                if (Guardar())
                    transaction.Commit();
                return 400;
            }
        }
        public int DeletePersona(int id)
        {
            _bd.Personas.Where(p => p.Id == id).ExecuteDelete();
            return 200;

        }
         
        public bool Guardar()
        {
            return _bd.SaveChanges() > 0;
        }
        
    }
    
}
