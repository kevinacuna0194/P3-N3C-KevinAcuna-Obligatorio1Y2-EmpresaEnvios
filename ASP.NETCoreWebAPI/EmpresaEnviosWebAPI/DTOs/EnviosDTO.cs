using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;

namespace EmpresaEnviosWebAPI.DTOs
{
    public class EnviosDTO
    {
        public IEnumerable<EnvioDTO> EnviosComunes { get; set; }
        public IEnumerable<EnvioDTO> EnviosUrgentes { get; set; }
    }
}
