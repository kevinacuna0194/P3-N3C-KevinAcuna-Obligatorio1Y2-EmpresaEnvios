using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarUsuarioPorId
    {
        UsuarioDTO Buscar(int id);
    }
}
