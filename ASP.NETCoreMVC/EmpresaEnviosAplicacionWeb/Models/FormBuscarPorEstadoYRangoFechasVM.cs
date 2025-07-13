using DTOs;
using Enum;
using System.ComponentModel.DataAnnotations;

namespace EmpresaEnviosAplicacionWeb.Models
{
    public class FormBuscarPorEstadoYRangoFechasVM
    {
        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> La fecha de fin es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Debe seleccionar al menos un estado.")]

        public IEnumerable<EstadoEnvio> EstadosEnvio { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Debe seleccionar un estado de envío.")]
        [Display(Name = "Estados del envío")]
        public int EstadoSeleccionado { get; set; }

        public EnviosVM? EnviosVM { get; set; } = null;

        public bool BusquedaEjecutada { get; set; } = false;
    }
}
