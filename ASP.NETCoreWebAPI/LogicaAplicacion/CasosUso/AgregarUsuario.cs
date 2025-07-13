using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class AgregarUsuario : IAgregarUsuario
    {
        public IRepositorioUsuario RepositorioUsuario { get; set; }

        public AgregarUsuario(IRepositorioUsuario repositorioUsuario)
        {
            RepositorioUsuario = repositorioUsuario;
        }
        public void Agregar(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO is null)
            {
                throw new DatosInvalidosException("El usuario no puede ser nulo");
            }

            RepositorioUsuario.Add(MapeadorUsuario.MapearUsuario(usuarioDTO));
        }

        public void Agregar(UsuarioDTO usuarioDTO, int idUsuarioLogueado)
        {
            if (usuarioDTO is null)
            {
                throw new DatosInvalidosException("El usuario no puede ser nulo");
            }

            if (idUsuarioLogueado <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario logueado no puede ser menor o igual a cero.");
            }

            Usuario usuario = MapeadorUsuario.MapearUsuario(usuarioDTO);

            RepositorioUsuario.Add(usuario, idUsuarioLogueado);
        }
    }
}
