using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record EmailUsuario
    {
        public string Email { get; init; }

        public EmailUsuario(string email)
        {
            Email = email;
            Validar();
        }

        public EmailUsuario()
        {

        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Email))
            {
                throw new DatosInvalidosException("El email es obligatorio");
            }

            if (!Email.Contains("@"))
            {
                throw new DatosInvalidosException("El email no es válido");
            }

            if (Email.Any(char.IsWhiteSpace))
            {
                throw new DatosInvalidosException("El email no puede contener espacios en blanco");
            }

            if (Email.Length < 5)
            {
                throw new DatosInvalidosException("El email debe tener al menos 5 caracteres");
            }

            if (Email.Length > 50)
            {
                throw new DatosInvalidosException("El email no puede tener más de 50 caracteres");
            }
        }
    }
}
