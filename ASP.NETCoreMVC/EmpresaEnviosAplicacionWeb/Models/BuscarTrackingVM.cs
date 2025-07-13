using DTOs;
using Enum;

namespace EmpresaEnviosAplicacionWeb.Models
{
    public class BuscarTrackingVM
    {
        public EnvioDTO EnvioDTO { get; set; }
        public TipoEnvio? TipoEnvio { get; set; }
        public bool BusquedaEjecutada { get; set; } = false;

        public void ConvertirPesoAKilogramos()
        {
            EnvioDTO.Peso = EnvioDTO.Peso / 1000m;
        }
    }
}
