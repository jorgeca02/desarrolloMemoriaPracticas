using ApiPersonas.Modelos;
using ApiPersonas.Modelos.Dto;

namespace ApiPersonas.Repositorio.IRepositorio
{
    public interface IPersonasRepositorio
    {
        List<PersonasDto> GetPersonas();
        PersonasDto GetPersonaPorId(int idPersona);
        ResponseDto AddPersona(PersonasDto personasDto);
        int PutPersona(PersonasDto personasDto);
        int DeletePersona(int idPersona);
        ResponseDto Login(string username, string password);
        bool Guardar();
    }
}
