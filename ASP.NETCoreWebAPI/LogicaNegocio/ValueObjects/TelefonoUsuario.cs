using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record TelefonoUsuario
    {
        public string Telefono { get; init; }

        public TelefonoUsuario(string telefono)
        {
            Telefono = telefono;
            Validar();
        }

        public TelefonoUsuario()
        {
        }

        public void Validar()
        {
            if (Telefono.Length <= 0)
            {
                throw new DatosInvalidosException("El Teléfono es obligatorio");
            }

            if (Telefono.Length < 9)
            {
                throw new DatosInvalidosException("El Teléfono debe tener al menos 9 dígitos");
            }

            if (Telefono.Length > 15)
            {
                throw new DatosInvalidosException("El Teléfono no puede tener más de 15 dígitos");
            }

            if (Telefono.Any(c => !char.IsDigit(c)))
            {
                throw new DatosInvalidosException("El Teléfono solo puede contener números");
            }

            if (Telefono.Any(char.IsWhiteSpace))
            {
                throw new DatosInvalidosException("El Teléfono no puede contener espacios en blanco");
            }
        }
    }
}
