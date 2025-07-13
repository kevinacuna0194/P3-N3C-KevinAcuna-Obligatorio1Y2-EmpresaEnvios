using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class ActualizarUsuario : IActualizarUsuario
    {
        public IRepositorioUsuario RepositorioUsuario { get; set; }

        public ActualizarUsuario(IRepositorioUsuario repositorioUsuario)
        {
            RepositorioUsuario = repositorioUsuario;
        }

        public void Actualizar(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO is null)
            {
                throw new DatosInvalidosException("El usuario no puede ser nulo.");
            }

            RepositorioUsuario.Update(MapeadorUsuario.MapearUsuario(usuarioDTO));
        }

        public void Actualizar(UsuarioDTO usuarioDTO, int idUsuarioLogueado)
        {
            if (usuarioDTO is null)
            {
                throw new DatosInvalidosException("El usuario no puede ser nulo.");
            }

            if (idUsuarioLogueado <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario logueado no puede ser menor o igual a cero.");
            }

            Usuario usuario = MapeadorUsuario.MapearUsuario(usuarioDTO);

            RepositorioUsuario.Update(usuario, idUsuarioLogueado);
        }
    }
}
