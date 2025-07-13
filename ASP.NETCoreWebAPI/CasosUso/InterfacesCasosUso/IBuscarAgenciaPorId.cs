using CasosUso.DTOs;

namespace CasosUso.InterfacesCasosUso
{
    public interface IBuscarAgenciaPorId
    {
        public AgenciaDTO Buscar(int idAgencia);
    }
}
