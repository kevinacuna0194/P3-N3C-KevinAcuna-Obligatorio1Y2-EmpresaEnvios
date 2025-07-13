using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record FechaNacimientoUsuario
    {
        public DateTime FechaNacimiento { get; init; }

        public FechaNacimientoUsuario(DateTime fechaNacimiento)
        {
            FechaNacimiento = fechaNacimiento;
            Validar();
        }

        public FechaNacimientoUsuario()
        {

        }

        public void Validar()
        {
            if (FechaNacimiento == DateTime.MinValue)
            {
                throw new DatosInvalidosException("La fecha de nacimiento es obligatoria");
            }

            if (FechaNacimiento >= DateTime.Now)
            {
                throw new DatosInvalidosException("La fecha de nacimiento no puede ser igual o mayor a la fecha actual");
            }

            if (FechaNacimiento < DateTime.Now.AddYears(-120))
            {
                throw new DatosInvalidosException("La fecha de nacimiento no puede ser menor a 120 años");
            }

            if (!EsMayorDeEdad(FechaNacimiento))
            {
                throw new DatosInvalidosException("El registro solo está disponible para personas mayores de edad. Verifica tu fecha de nacimiento.");
            }
        }

        public bool EsMayorDeEdad(DateTime fechaNacimiento)
        {
            int edad = DateTime.Today.Year - fechaNacimiento.Year;

            // Ajustar si aún no ha cumplido años este año
            if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad))
            {
                edad--;
            }

            return edad >= 18;
        }

    }
}
