using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IAgregarEnvio
    {
        void Agregar(EnvioDTO envioDTO, int idUsuarioLogueado);
    }
}
