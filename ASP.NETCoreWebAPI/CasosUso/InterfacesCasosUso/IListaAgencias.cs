using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IListaAgencias
    {
        IEnumerable<AgenciaDTO> ObtenerListaAgencias();
    }
}
