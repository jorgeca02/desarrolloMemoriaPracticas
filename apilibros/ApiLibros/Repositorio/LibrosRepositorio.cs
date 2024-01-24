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

namespace ApiLibros.Repositorio
{
    public class LibrosRepositorio : ILibrosRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;
        public LibrosRepositorio(ApplicationDbContext bd, IMapper mapper) 
        {
            _bd = bd;
            _mapper = mapper;
        }
        public List<LibroDto> GetListaLibros(int autor, int editorial, int persona, string titulo)
        {
            var listaLibroDto = new List<LibroDto>();
            var listaLibro = _bd.Libros.ToList();
            if (autor != 0)
            {
                listaLibro = listaLibro.Where(l => l.AutorId == autor).ToList();
            }
            if (editorial != 0)
            {
                listaLibro = listaLibro.Where(l => l.EditorialId == editorial).ToList();
            }
            if (persona != 0)
            {
                listaLibro = listaLibro.Where(l => l.IdPersona == persona).ToList();
            }
            if (titulo != null)
            {
                listaLibro = listaLibro.Where(l => l.Titulo.ToLower().Contains(titulo.ToLower())).ToList();
            }
            foreach (var libro in listaLibro)
            {
                var libroDto = new LibroDto();
                _mapper.Map(libro, libroDto);
                listaLibroDto.Add(libroDto);
            }
            return listaLibroDto;
        }
        public LibroDto GetLibroPorId(int id)
        {
            var libroDto= new LibroDto();
            _mapper.Map(_bd.Libros.FirstOrDefault(l=>l.Id==id), libroDto);
            return libroDto;
        }
        public ResponseDto AddLibro(LibroDto libro) 
        {
            using var transaction = _bd.Database.BeginTransaction();
            var libroDb = new Libros();
            _bd.Libros.Add(_mapper.Map(libro, libroDb));
            
            if (Guardar() ) 
            {
                transaction.Commit();
                return new ResponseDto
                {
                    CodigoOperacion = 200,
                    DescripcionOperacion = "Libro añadido a bbdd",
                    Mensaje = "Añadido correctamente",
                    IdObjeto = libroDb.Id.ToString()
                };
            }
            return new ResponseDto
            {
                CodigoOperacion = 400,
                DescripcionOperacion = "error",
                Mensaje = "error"
            }; ;
            
        }
        public int PutLibro(LibroDto libroDto)
        {
            using var transaction = _bd.Database.BeginTransaction();
            var libro = _bd.Libros.FirstOrDefault(l => l.Id == libroDto.Id);
            if (libro == null)
            {
                return 404;
            }
            else
            {
                _mapper.Map(libroDto,libro);
                libro.AutorId = libroDto.Autor.Id;
                libro.EditorialId = libroDto.Editorial.Id;
                if(Guardar())
                    transaction.Commit();
                return 400;
            }
        }
        public int DeleteLibro(int id)
        {
            _bd.Libros.Where(l => l.Id == id).ExecuteDelete();
            return 200;

        }
        public bool Guardar()
        {
            return _bd.SaveChanges() > 0;
        }

    }

}
