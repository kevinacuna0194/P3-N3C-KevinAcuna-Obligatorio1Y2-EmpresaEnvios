using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record DireccionUsuario
    {
        public string Direccion { get; init; }

        public DireccionUsuario(string direccion)
        {
            Direccion = direccion;
            Validar();
        }

        public DireccionUsuario()
        {

        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Direccion))
            {
                throw new DatosInvalidosException("La Dirección es obligatoria");
            }

            if (Direccion.Length < 10)
            {
                throw new DatosInvalidosException("La Dirección debe tener al menos 10 caracteres");
            }

            if (Direccion.Length > 50)
            {
                throw new DatosInvalidosException("La Dirección no puede tener más de 50 caracteres");
            }

            if (Direccion.Any(char.IsControl))
            {
                throw new DatosInvalidosException("La Dirección no puede contener caracteres de control");
            }

            if (Direccion.Any(char.IsSymbol))
            {
                throw new DatosInvalidosException("La Dirección no puede contener símbolos");
            }

            //if (Direccion.Any(char.IsPunctuation))
            //{
            //    throw new DatosInvalidosException("La Dirección no puede contener caracteres especiales o de puntuación");
            //}
        }
    }
}
