using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IListaEnviosComunes
    {
        IEnumerable<EnvioDTO> ObtenerEnviosComunes();
    }
}
