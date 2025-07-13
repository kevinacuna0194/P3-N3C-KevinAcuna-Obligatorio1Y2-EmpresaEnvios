using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class ListaUsuarios : IListaUsuarios
    {
        public IRepositorioUsuario RepositorioUsuario { get; set; }

        public ListaUsuarios(IRepositorioUsuario repositorioUsuario)
        {
            RepositorioUsuario = repositorioUsuario;
        }
        public List<UsuarioDTO> ObtenerListaUsuarios()
        {
            List<Usuario> usuarios = RepositorioUsuario.FindAll();

            if (usuarios.Count == 0 || !usuarios.Any())
            {
                throw new DatosInvalidosException("No hay usuarios registrados.");
            }

            List<UsuarioDTO> usuariosDTO = MapeadorUsuario.MapearListaUsuarioDTO(usuarios);

            if (usuariosDTO.Count == 0 || !usuariosDTO.Any())
            {
                throw new DatosInvalidosException("No se pudo mapear la lista de usuarios a DTOs.");
            }

            return usuariosDTO;
        }
    }
}
