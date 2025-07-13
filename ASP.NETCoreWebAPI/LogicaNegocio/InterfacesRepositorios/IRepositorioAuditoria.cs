using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioAuditoria : IRepositorio<Auditoria>
    {
        // Métodos específicos para el repositorio de auditoría
        List<Auditoria> ObtenerAuditoriasPorUsuario(int usuarioId);
        List<Auditoria> ObtenerAuditoriasPorAccion(AccionAuditoria accion);
        List<Auditoria> ObtenerAuditoriasPorRangoFechas(DateTime fechaInicio, DateTime fechaFin);
        List<Auditoria> ObtenerAuditoriasPorAccionYUsuario(AccionAuditoria accion, int usuarioId);
        List<Auditoria> ObtenerAuditoriasPorAccionYRangoFechas(AccionAuditoria accion, DateTime fechaInicio, DateTime fechaFin);
    }
}
