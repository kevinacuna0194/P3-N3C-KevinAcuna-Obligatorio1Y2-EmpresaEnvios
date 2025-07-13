using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IActualizarUsuario
    {
        void Actualizar(UsuarioDTO usuarioDTO, int idUsuarioLogueado);
    }
}
