using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IActualizarEstado
    {
        void Actualizar(EnvioDTO envioDTO, int idUsuarioLogueado, string comentario);
    }
}
