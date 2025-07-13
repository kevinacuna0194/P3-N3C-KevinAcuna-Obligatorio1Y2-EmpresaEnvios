using CasosUso.DTOs;
using Enum;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarEnvioPorId
    {
        EnvioDTO Buscar(int id, TipoEnvio tipoEnvio);
        EnvioDTO Buscar(int id);
    }
}
