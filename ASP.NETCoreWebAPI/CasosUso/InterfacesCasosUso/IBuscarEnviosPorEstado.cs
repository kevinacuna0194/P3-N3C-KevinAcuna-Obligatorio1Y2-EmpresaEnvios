using CasosUso.DTOs;
using Enum;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarEnviosPorEstado
    {
        IEnumerable<EnvioDTO> Buscar(EstadoEnvio estado);
    }
}
