using LogicaAccesoDatos.EntityFramework;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioAuditoriaEF : IRepositorioAuditoria
    {
        public LibraryContext LibraryContext { get; set; }

        public RepositorioAuditoriaEF(LibraryContext libraryContext)
        {
            LibraryContext = libraryContext;
        }

        public List<Auditoria> ObtenerAuditoriasPorUsuario(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<Auditoria> ObtenerAuditoriasPorAccion(AccionAuditoria accion)
        {
            throw new NotImplementedException();
        }

        public List<Auditoria> ObtenerAuditoriasPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public List<Auditoria> ObtenerAuditoriasPorAccionYUsuario(AccionAuditoria accion, int usuarioId)
        {
            throw new NotImplementedException();
        }

        public List<Auditoria> ObtenerAuditoriasPorAccionYRangoFechas(AccionAuditoria accion, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public void Add(Auditoria entidad)
        {
            throw new NotImplementedException();
        }

        public void Update(Auditoria entidad)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Auditoria FindById(int id)
        {
            return LibraryContext.Auditorias
                .Include(a => a.Usuario)
                .Include(a => a.Detalle)
                .FirstOrDefault(a => a.Id == id);
        }

        public List<Auditoria> FindAll()
        {
            return LibraryContext.Auditorias
                .Include(a => a.Usuario)
                .Include(a => a.Detalle)
                .OrderBy(a => a.Id)
                .ToList();
        }
    }
}
