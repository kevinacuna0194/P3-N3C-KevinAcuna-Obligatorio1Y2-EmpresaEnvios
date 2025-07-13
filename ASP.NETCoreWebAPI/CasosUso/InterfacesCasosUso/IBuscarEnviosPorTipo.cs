using CasosUso.DTOs;
using Enum;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarEnviosPorTipo
    {
        IEnumerable<EnvioDTO> Buscar(TipoEnvio tipoEnvio);
    }
}
