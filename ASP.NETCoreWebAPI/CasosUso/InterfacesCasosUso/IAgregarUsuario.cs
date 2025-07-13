using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IAgregarUsuario
    {
        void Agregar(UsuarioDTO usuarioDTO, int idUsuarioLogueado);
    }
}
