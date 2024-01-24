using ApiEditoriales.Repositorio.IRepositorio;
using ApiEditoriales.Modelos.Dto;
using ApiLibros.Data;
using ApiLibros.Modelos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiEditoriales.Repositorio
{
    public class EditorialesRepositorio : IEditorialesRepositorio
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;
        public EditorialesRepositorio(ApplicationDbContext bd, IMapper mapper) 
        {
            _bd = bd;
            _mapper = mapper;
        }
        public List<EditorialDto> GetListaEditorial()
        {
            var listaEditorialesDto = new List<EditorialDto>();
            _mapper.Map(_bd.Editoriales, listaEditorialesDto);
            return listaEditorialesDto;
        }
        public EditorialDto GetEditorialPorId(int id)
        {
            var editorialDto= new EditorialDto();
            _mapper.Map(_bd.Editoriales.FirstOrDefault(e=>e.Id==id), editorialDto);
            return editorialDto;
        }
        public int AddEditorial(EditorialDto editorial) 
        {
            var existe = _bd.Editoriales.Any(e => e.Nombre == editorial.Nombre);
            if (existe)
            {
                return 401;
            }
            using var transaction = _bd.Database.BeginTransaction();
            var editorialDb = new Editoriales();
            _bd.Editoriales.Add(_mapper.Map(editorial, editorialDb));
            if (Guardar() ) 
            {
                transaction.Commit();
                return 200; 
            }
            return 0;
            
        }
        public int PutEditorial(EditorialDto editorialesDto)
        {
            using var transaction = _bd.Database.BeginTransaction();
            var editoriales = _bd.Editoriales.FirstOrDefault(e => e.Id == editorialesDto.Id);
            if (editoriales == null)
            {
                return 404;
            }
            else
            {
                _mapper.Map(editorialesDto,editoriales);
                if(Guardar())
                    transaction.Commit();
                return 400;
            }
        }
        public int DeleteEditorial(int id)
        {
            _bd.Editoriales.Where(l => l.Id == id).ExecuteDelete();
            return 200;

        }
        public bool Guardar()
        {
            return _bd.SaveChanges() > 0;
        }

    }

}
