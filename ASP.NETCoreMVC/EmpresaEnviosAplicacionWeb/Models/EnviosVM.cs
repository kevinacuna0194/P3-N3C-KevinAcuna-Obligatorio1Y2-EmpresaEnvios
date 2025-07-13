using DTOs;

namespace EmpresaEnviosAplicacionWeb.Models
{
    public class EnviosVM
    {
        public IEnumerable<EnvioDTO> EnviosComunes { get; set; }
        public IEnumerable<EnvioDTO> EnviosUrgentes { get; set; }

        public void ConvertirPesoAKilogramos()
        {
            foreach (var envio in EnviosComunes)
                envio.Peso = envio.Peso / 1000m;

            foreach (var envio in EnviosUrgentes)
                envio.Peso = envio.Peso / 1000m;
        }
    }
}
