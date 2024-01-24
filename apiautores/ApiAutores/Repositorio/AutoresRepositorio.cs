
using ApiAutor.Modelos;
using ApiAutor.Modelos.Dto;
using ApiAutores.Repositorio.IRepositorio;
using ApiLibros.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiAutor.Repositorio
{
    public class AutoresRepositorio : IAutoresRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;
        public AutoresRepositorio(ApplicationDbContext bd, IMapper mapper) 
        {
            _bd = bd;
            _mapper = mapper;
        }
        public List<AutorDto> GetListaAutores()
        {
            var listaAutoresDto = new List<AutorDto>();
            _mapper.Map(_bd.Autores.ToList(), listaAutoresDto);
            return listaAutoresDto;
        }
        public AutorDto GetAutorPorId(int id)
        {
            var autorDto= new AutorDto();
            _mapper.Map(_bd.Autores.ToList().FirstOrDefault(l=>l.Id==id), autorDto);
            return autorDto;
        }
        public int AddAutor(AutorDto autor) 
        {
            using var transaction = _bd.Database.BeginTransaction();
            var autorDb = new Autores();
            _bd.Autores.Add(_mapper.Map(autor, autorDb));
            if (Guardar() ) 
            {
                transaction.Commit();
                return 200; 
            }
            return 0;
            
        }
        public int PutAutor(AutorDto autorDto)
        {
            using var transaction = _bd.Database.BeginTransaction();
            var autor = _bd.Autores.FirstOrDefault(a => a.Id == autorDto.Id);
            if (autor == null)
            {
                return 404;
            }
            else
            {
                _mapper.Map(autorDto,autor);
                if(Guardar())
                    transaction.Commit();
                return 400;
            }
        }
        public int DeleteAutor(int id)
        {
            _bd.Autores.Where(a => a.Id == id).ExecuteDelete();
            return 200;

        }
        public bool Guardar()
        {
            return _bd.SaveChanges() > 0;
        }

    }

}
