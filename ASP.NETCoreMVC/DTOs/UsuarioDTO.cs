using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Debes ingresar tu nombre.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresa tu correo electrónico.")]
        [EmailAddress(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> El formato del correo no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> La contraseña es obligatoria.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresa la fecha de registro.")]
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresa tu fecha de nacimiento.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Ingresa tu número de teléfono.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> La dirección es obligatoria.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Debes ingresar tu documento de identidad.")]
        public int DocumentoIdentidad { get; set; }

        [Required(ErrorMessage = "<i class='bi bi-exclamation-circle-fill me-1'></i> Selecciona un rol.")]
        public int RolId { get; set; }

        public string NombreRol { get; set; }
    }
}
