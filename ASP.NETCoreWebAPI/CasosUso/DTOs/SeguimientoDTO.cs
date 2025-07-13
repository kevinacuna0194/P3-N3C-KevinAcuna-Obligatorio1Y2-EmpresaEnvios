using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasosUso.DTOs
{
    public class SeguimientoDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int? EnvioId { get; set; }
        public int EmpleadoId { get; set; }
        public string? NombreEmpleado { get; set; }
        public string Comentario { get; set; }
    }
}
