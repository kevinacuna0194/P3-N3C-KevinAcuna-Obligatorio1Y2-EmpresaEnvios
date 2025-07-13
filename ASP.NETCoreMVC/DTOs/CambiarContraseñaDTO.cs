using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class CambiarContraseñaDTO
    {
        public string Email { get; set; }
        public string ContraseñaActual { get; set; }
        public string NuevaContraseña { get; set; }
        public int IdUsuarioLogueado { get; set; }
    }
}
