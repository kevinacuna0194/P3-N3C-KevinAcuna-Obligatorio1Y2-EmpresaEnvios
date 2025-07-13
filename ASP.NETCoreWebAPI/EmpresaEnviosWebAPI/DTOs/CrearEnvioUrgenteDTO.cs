using CasosUso.DTOs;

namespace EmpresaEnviosWebAPI.DTOs
{
    public class CrearEnvioUrgenteDTO
    {
        public int UsuarioId { get; set; }
        public string EmailCliente { get; set; }
        public decimal Peso { get; set; }
        public string DireccionPostal { get; set; }
    }
}
