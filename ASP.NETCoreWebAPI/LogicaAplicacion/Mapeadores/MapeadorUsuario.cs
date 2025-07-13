using CasosUso.DTOs;
using ExcepcionesPropias;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaAplicacion.Mapeadores
{
    public class MapeadorUsuario
    {
        public static Usuario MapearUsuario(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO is null)
            {
                throw new DatosInvalidosException("El UsuarioDTO no puede ser nulo.");
            }

            Usuario usuario = null;

            return usuario = new Usuario
            {
                Id = usuarioDTO.Id,
                Nombre = new NombreUsuario(usuarioDTO.Nombre),
                Email = new EmailUsuario(usuarioDTO.Email),
                Password = new PasswordUsuario(usuarioDTO.Password),
                FechaRegistro = new FechaRegistroUsuario(usuarioDTO.FechaRegistro),
                FechaNacimiento = new FechaNacimientoUsuario(usuarioDTO.FechaNacimiento),
                Telefono = new TelefonoUsuario(usuarioDTO.Telefono),
                Direccion = new DireccionUsuario(usuarioDTO.Direccion),
                DocumentoIdentidad = new DocumentoIdentidadUsuario(usuarioDTO.DocumentoIdentidad),
                Rol = new Rol() { Id = usuarioDTO.RolId }
            };
        }

        public static UsuarioDTO MapearUsuarioDTO(Usuario usuario)
        {
            if (usuario is null)
            {
                throw new DatosInvalidosException("El Usuario no puede ser nulo");
            }

            UsuarioDTO usuarioDTO = null;

            return usuarioDTO = new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre.Nombre,
                Email = usuario.Email.Email,
                Password = usuario.Password.Password,
                FechaRegistro = usuario.FechaRegistro.FechaRegistro,
                FechaNacimiento = usuario.FechaNacimiento.FechaNacimiento,
                Telefono = usuario.Telefono.Telefono,
                Direccion = usuario.Direccion.Direccion,
                DocumentoIdentidad = usuario.DocumentoIdentidad.DocumentoIdentidad,
                RolId = usuario.Rol.Id,
                NombreRol = usuario.Rol.Nombre.Nombre
            };
        }


        public static List<UsuarioDTO> MapearListaUsuarioDTO(List<Usuario> usuarios)
        {
            if (usuarios is null || !usuarios.Any())
            {
                throw new DatosInvalidosException("El listado de usuarios no puede ser nulo o vacío");
            }

            List<UsuarioDTO> listaUsuarioDTO = new List<UsuarioDTO>();

            foreach (Usuario usuario in usuarios)
            {
                listaUsuarioDTO.Add(MapearUsuarioDTO(usuario));
            }

            return listaUsuarioDTO;
        }
    }
}
