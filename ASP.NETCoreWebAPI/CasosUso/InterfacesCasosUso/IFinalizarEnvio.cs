using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IFinalizarEnvio
    {
        void Finalizar(EnvioDTO envioDTO, int idUsuarioLogueado);
    }
}
