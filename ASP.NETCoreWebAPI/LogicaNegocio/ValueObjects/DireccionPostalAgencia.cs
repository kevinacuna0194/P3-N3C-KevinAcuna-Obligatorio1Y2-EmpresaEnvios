using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record DireccionPostalAgencia
    {
        public string DireccionPostal { get; init; }

        public DireccionPostalAgencia(string direccionPostal)
        {
            DireccionPostal = direccionPostal;
            Validar();
        }

        public DireccionPostalAgencia()
        {
        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(DireccionPostal))
            {
                throw new DatosInvalidosException("La dirección postal es obligatoria");
            }
            if (DireccionPostal.Length < 5)
            {
                throw new DatosInvalidosException("La dirección postal debe tener al menos 5 caracteres");
            }
            if (DireccionPostal.Length > 50)
            {
                throw new DatosInvalidosException("La dirección postal no puede tener más de 50 caracteres");
            }
            if (DireccionPostal.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new DatosInvalidosException("La dirección postal solo puede contener letras y números");
            }

            if (DireccionPostal.Any(c => char.IsControl(c)))
            {
                throw new DatosInvalidosException("La dirección postal no puede contener caracteres de control");
            }
        }
    }
}
