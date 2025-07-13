using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record NombreAgencia
    {
        public string Nombre { get; init; }

        public NombreAgencia(string nombre)
        {
            Nombre = nombre;
            Validar();
        }

        public NombreAgencia()
        {

        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Nombre))
            {
                throw new DatosInvalidosException("El nombre es obligatorio");
            }

            if (Nombre.Length < 3)
            {
                throw new DatosInvalidosException("El Nombre debe tener al menos 3 caracteres");
            }

            if (Nombre.Length > 20)
            {
                throw new DatosInvalidosException("El Nombre no puede tener más de 20 caracteres");
            }

            if (Nombre.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new DatosInvalidosException("El Nombre solo puede contener letras y números");
            }

            if (!Nombre.Any(char.IsUpper))
            {
                throw new DatosInvalidosException("El Nombre debe contener al menos una letra mayúscula");
            }

            //if (Nombre.Any(char.IsWhiteSpace))
            //{
            //    throw new DatosInvalidosException("El Nombre no puede contener espacios en blanco");
            //}

            if (Nombre.Any(char.IsControl))
            {
                throw new DatosInvalidosException("El Nombre no puede contener caracteres de control");
            }

            if (Nombre.Any(char.IsSymbol))
            {
                throw new DatosInvalidosException("El Nombre no puede contener símbolos");
            }

            if (Nombre.Any(char.IsPunctuation))
            {
                throw new DatosInvalidosException("El Nombre no puede contener caracteres especiales o de puntuación");
            }
        }
    }
}
