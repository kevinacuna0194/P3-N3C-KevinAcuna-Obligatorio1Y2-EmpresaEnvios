using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class ListaRoles : IListaRoles
    {
        public IRepositorioRol RepositorioRol { get; set; }

        public ListaRoles(IRepositorioRol repositorioRol)
        {
            RepositorioRol = repositorioRol;
        }

        public List<RolDTO> ObtenerListaRoles()
        {
            List<Rol> roles = RepositorioRol.FindAll();

            if (roles == null)
            {
                throw new DatosInvalidosException("No se encontraron roles");
            }

            List<RolDTO> rolesDTO = MapeadorRol.MapearListaRolDTO(roles);

            if (rolesDTO == null)
            {
                throw new DatosInvalidosException("No se pudo mapear la lista de roles a DTOs");
            }

            return rolesDTO;
        }
    }
}
