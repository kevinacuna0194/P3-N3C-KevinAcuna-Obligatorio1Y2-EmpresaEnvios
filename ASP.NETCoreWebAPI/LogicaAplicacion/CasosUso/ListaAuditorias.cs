using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class ListaAuditorias : IListaAuditorias
    {
        public IRepositorioAuditoria RepositorioAuditoria { get; set; }

        public ListaAuditorias(IRepositorioAuditoria repositorioAuditoria)
        {
            RepositorioAuditoria = repositorioAuditoria;
        }

        public List<AuditoriaDTO> ObtenerListaAuditorias()
        {
            return MapeadorAuditoria.MapearListaAuditoriaDTO(RepositorioAuditoria.FindAll());
        }
    }
}
