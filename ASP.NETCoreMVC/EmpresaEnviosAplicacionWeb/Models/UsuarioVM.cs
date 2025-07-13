using DTOs;

namespace EmpresaEnviosAplicacionWeb.Models
{
    public class UsuarioVM
    {
        public UsuarioDTO UsuarioDTO { get; set; }
        public IEnumerable<RolDTO> Roles { get; set; }
    }
}
