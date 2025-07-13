using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarAuditoriaPorId
    {
        AuditoriaDTO Buscar(int id);
    }
}
