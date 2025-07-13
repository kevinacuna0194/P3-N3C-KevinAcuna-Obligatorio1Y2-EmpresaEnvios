using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarUsuarioPorEmail : IBuscarUsuarioPorEmail
    {
        public IRepositorioUsuario RepositorioUsuario { get; set; }

        public BuscarUsuarioPorEmail(IRepositorioUsuario repositorioUsuario)
        {
            RepositorioUsuario = repositorioUsuario;
        }

        public UsuarioDTO Buscar(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new DatosInvalidosException("El email no puede ser nulo o vacío");
            }

            Usuario usuario = RepositorioUsuario.ObtenerUsuarioPorEmail(email);

            if (usuario == null)
            {
                throw new DatosInvalidosException("No se encontró un usuario con ese email");
            }

            UsuarioDTO usuarioDTO = MapeadorUsuario.MapearUsuarioDTO(usuario);

            if (usuarioDTO == null)
            {
                throw new DatosInvalidosException("Error al mapear el usuario a UsuarioDTO");
            }

            return usuarioDTO;
        }
    }
}
