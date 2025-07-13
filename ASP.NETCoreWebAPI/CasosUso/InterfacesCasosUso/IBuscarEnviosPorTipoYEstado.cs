using CasosUso.DTOs;
using Enum;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarEnviosPorTipoYEstado
    {
        IEnumerable<EnvioDTO> Buscar(TipoEnvio tipoEnvio, EstadoEnvio estadoEnvio);
    }
}
