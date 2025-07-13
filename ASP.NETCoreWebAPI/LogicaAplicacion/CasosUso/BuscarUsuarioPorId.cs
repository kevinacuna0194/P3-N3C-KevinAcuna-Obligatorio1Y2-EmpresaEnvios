using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarUsuarioPorId : IBuscarUsuarioPorId
    {
        public IRepositorioUsuario RepositorioUsuario { get; set; }

        public BuscarUsuarioPorId(IRepositorioUsuario repositorioUsuario)
        {
            RepositorioUsuario = repositorioUsuario;
        }

        public UsuarioDTO Buscar(int id)
        {
            if (id <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario no puede ser menor o igual a cero.");
            }

            Usuario usuario = RepositorioUsuario.FindById(id);

            if (usuario is null)
            {
                throw new DatosInvalidosException($"No se encontró un usuario con el ID: {id}");
            }

            UsuarioDTO usuarioDTO = MapeadorUsuario.MapearUsuarioDTO(usuario);

            if (usuarioDTO is null)
            {
                throw new DatosInvalidosException("Error al mapear el usuario a UsuarioDTO");
            }

            return usuarioDTO;
        }
    }
}
