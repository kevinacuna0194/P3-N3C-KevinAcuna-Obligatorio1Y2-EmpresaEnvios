using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record DireccionPostalEnvioUrgente
    {
        public string DireccionPostal { get; init; }

        public DireccionPostalEnvioUrgente(string direccionPostal)
        {
            DireccionPostal = direccionPostal;
            Validar();
        }

        public DireccionPostalEnvioUrgente()
        {

        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(DireccionPostal))
            {
                throw new DatosInvalidosException("La dirección postal no puede ser nula o vacía");
            }

            if (DireccionPostal.Length < 5)
            {
                throw new DatosInvalidosException("La dirección postal debe tener al menos 5 caracteres");
            }

            if (DireccionPostal.Length > 100)
            {
                throw new DatosInvalidosException("La dirección postal no puede exceder los 100 caracteres");
            }

            if (DireccionPostal.Any(c => char.IsControl(c)))
            {
                throw new DatosInvalidosException("La dirección postal no puede contener caracteres de control");
            }
        }
    }
}
