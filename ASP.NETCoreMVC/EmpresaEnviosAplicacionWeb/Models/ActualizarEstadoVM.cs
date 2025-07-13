using Enum;
using System.ComponentModel.DataAnnotations;

namespace EmpresaEnviosAplicacionWeb.Models
{
    public class ActualizarEstadoVM
    {
        public int Id { get; set; }

        public TipoEnvio TipoEnvio { get; set; }

        public IEnumerable<EstadoEnvio> EstadosEnvio { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> El comentario no puede estar vacío.")]
        [StringLength(500, ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> El comentario no puede superar los 500 caracteres.")]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Debe seleccionar un estado de envío.")]
        public int? EstadoSeleccionado { get; set; }
    }
}