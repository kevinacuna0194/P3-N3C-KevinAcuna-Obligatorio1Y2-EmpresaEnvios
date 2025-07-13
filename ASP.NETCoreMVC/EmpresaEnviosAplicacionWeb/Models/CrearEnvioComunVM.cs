using DTOs;

namespace EmpresaEnviosAplicacionWeb.Models
{
    public class CrearEnvioComunVM
    {
        public EnvioDTO EnvioDTO { get; set; }
        public IEnumerable<AgenciaDTO> Agencias { get; set; }
    }
}
