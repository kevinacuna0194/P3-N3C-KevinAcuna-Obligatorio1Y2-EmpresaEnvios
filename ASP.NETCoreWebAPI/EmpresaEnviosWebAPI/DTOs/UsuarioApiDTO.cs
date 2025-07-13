using CasosUso.DTOs;

namespace EmpresaEnviosWebAPI.DTOs
{
    public class UsuarioApiDTO
    {
        public UsuarioDTO usuarioDTO { get; set; }
        public int RolId { get; set; }
        public int IdUsuarioLogueado { get; set; }
    }
}
