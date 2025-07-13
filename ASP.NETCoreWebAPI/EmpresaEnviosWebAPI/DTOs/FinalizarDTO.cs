using Enum;

namespace EmpresaEnviosWebAPI.DTOs
{
    public class FinalizarDTO
    {
        public int UsuarioId { get; set; }
        public TipoEnvio TipoEnvio { get; set; }
    }
}
