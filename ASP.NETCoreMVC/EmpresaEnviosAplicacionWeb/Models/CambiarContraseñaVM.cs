using System.ComponentModel.DataAnnotations;

namespace EmpresaEnviosAplicacionWeb.Models
{
    public class CambiarContraseñaVM
    {
        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresá tu contraseña actual.")]
        [DataType(DataType.Password)]
        public string ActualPassword { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresá la nueva contraseña.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> La nueva contraseña debe tener al menos {2} caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> La nueva contraseña debe contener al menos una mayúscula y un número.")]
        public string NuevaPassword { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Confirmá la nueva contraseña.")]
        [DataType(DataType.Password)]
        [Compare("NuevaPassword", ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Las contraseñas no coinciden.")]
        public string ConfirmarPassword { get; set; }
    }
}
