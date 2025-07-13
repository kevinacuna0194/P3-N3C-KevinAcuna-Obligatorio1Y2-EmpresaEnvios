using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record PasswordUsuario
    {
        public string Password { get; init; }

        public PasswordUsuario(string contrasena)
        {
            Password = contrasena;
            Validar();
        }

        public PasswordUsuario()
        {

        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Password))
            {
                throw new DatosInvalidosException("La contraseña es obligatoria");
            }

            if (Password.Length < 5)
            {
                throw new DatosInvalidosException("La contraseña debe tener al menos 5 caracteres");
            }

            if (Password.Length > 20)
            {
                throw new DatosInvalidosException("La contraseña no puede tener más de 20 caracteres");
            }

            if (!Password.Any(char.IsDigit))
            {
                throw new DatosInvalidosException("La contraseña debe contener al menos un número");
            }

            if (!Password.Any(char.IsUpper))
            {
                throw new DatosInvalidosException("La contraseña debe contener al menos una letra mayúscula");
            }

            if (Password.Any(char.IsWhiteSpace))
            {
                throw new DatosInvalidosException("La contraseña no puede contener espacios en blanco");
            }

            //if (Contrasena.Any(c => !char.IsLetterOrDigit(c)))
            //{
            //    throw new DatosInvalidosException("La contraseña solo puede contener letras y números");
            //}

            if (Password.Any(c => char.IsControl(c)))
            {
                throw new DatosInvalidosException("La contraseña no puede contener caracteres de control");
            }
        }
    }
}
