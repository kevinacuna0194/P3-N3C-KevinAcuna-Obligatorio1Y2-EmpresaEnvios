using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IListaEnviosUrgentes
    {
        IEnumerable<EnvioDTO> ObtenerEnviosUrgentes();
    }
}
