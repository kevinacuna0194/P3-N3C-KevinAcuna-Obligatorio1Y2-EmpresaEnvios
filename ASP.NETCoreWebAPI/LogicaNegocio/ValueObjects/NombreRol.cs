using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record NombreRol
    {
        public string Nombre { get; init; }

        public NombreRol(string nombre)
        {
            Nombre = nombre;
            Validar();
        }

        public NombreRol()
        {

        }

        private void Validar()
        {
            if (string.IsNullOrEmpty(Nombre))
            {
                throw new ArgumentException("El Nombre del Rol es obligatorio");
            }

            if (Nombre.Length < 3)
            {
                throw new ArgumentException("El Nombre del Rol debe tener al menos 3 caracteres");
            }

            if (Nombre.Length > 20)
            {
                throw new ArgumentException("El Nombre del Rol no puede tener más de 20 caracteres");
            }

            if (Nombre.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new ArgumentException("El Nombre del Rol solo puede contener letras y números");
            }

            if (!Nombre.Any(char.IsUpper))
            {
                throw new ArgumentException("El Nombre del Rol debe contener al menos una letra mayúscula");
            }

            if (Nombre.Any(char.IsWhiteSpace))
            {
                throw new ArgumentException("El Nombre del Rol no puede contener espacios en blanco");
            }

            if (Nombre.Any(c => char.IsPunctuation(c)))
            {
                throw new ArgumentException("El Nombre del Rol no puede contener caracteres de puntuación");
            }

            if (Nombre.Any(c => char.IsControl(c)))
            {
                throw new ArgumentException("El Nombre del Rol no puede contener caracteres de control");
            }

            if (Nombre.Any(c => char.IsSymbol(c)))
            {
                throw new ArgumentException("El Nombre del Rol no puede contener símbolos");
            }
        }
    }
}
