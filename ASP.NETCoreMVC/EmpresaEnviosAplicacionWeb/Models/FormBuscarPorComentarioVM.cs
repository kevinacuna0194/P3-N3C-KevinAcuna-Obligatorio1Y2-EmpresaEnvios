using System.ComponentModel.DataAnnotations;

namespace EmpresaEnviosAplicacionWeb.Models
{
    public class FormBuscarPorComentarioVM
    {
        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Debe ingresar una palabra clave para buscar envíos.")]
        [StringLength(100, ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> El comentario no puede superar los 100 caracteres.")]
        [Display(Name = "Comentario")]
        public string Comentario { get; set; }
        public EnviosVM? EnviosVM { get; set; } = null;
        public bool BusquedaEjecutada { get; set; } = false;
    }
}
