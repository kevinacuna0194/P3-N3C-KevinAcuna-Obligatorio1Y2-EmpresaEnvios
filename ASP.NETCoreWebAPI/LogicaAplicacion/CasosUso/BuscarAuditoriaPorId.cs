using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarAuditoriaPorId : IBuscarAuditoriaPorId
    {
        public IRepositorioAuditoria RepositorioAuditoria { get; set; }
        public BuscarAuditoriaPorId(IRepositorioAuditoria repositorioAuditoria)
        {
            RepositorioAuditoria = repositorioAuditoria;
        }

        public AuditoriaDTO Buscar(int id)
        {
            if (id <= 0)
            {
                throw new DatosInvalidosException("El Id de la Auditoria no puede ser menor o igual a cero");
            }

            Auditoria auditoria = RepositorioAuditoria.FindById(id);

            if (auditoria is null)
            {
                throw new DatosInvalidosException("No se encontró la auditoria con el ID proporcionado");
            }

            AuditoriaDTO auditoriaDTO = MapeadorAuditoria.MapearAuditoriaDTO(auditoria);

            return auditoriaDTO;
        }
    }
}
