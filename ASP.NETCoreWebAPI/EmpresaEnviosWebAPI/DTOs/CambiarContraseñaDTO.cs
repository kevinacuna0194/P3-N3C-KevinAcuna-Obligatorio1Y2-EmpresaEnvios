using CasosUso.DTOs;

namespace EmpresaEnviosWebAPI.DTOs
{
    public class CambiarContraseñaDTO
    {
        public string Email { get; set; }
        public string ContraseñaActual { get; set; }
        public string NuevaContraseña { get; set; }
        public int IdUsuarioLogueado { get; set; }
    }
}
