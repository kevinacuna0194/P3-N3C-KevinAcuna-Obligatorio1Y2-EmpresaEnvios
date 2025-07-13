using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IListaEnvios
    {
        IEnumerable<EnvioDTO> ObtenerListaEnvios();
    }
}
