using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarUsuarioPorEmail
    {
        UsuarioDTO Buscar(string email);
    }
}
