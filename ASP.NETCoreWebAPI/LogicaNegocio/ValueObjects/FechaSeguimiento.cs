using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record FechaSeguimiento
    {
        public DateTime Fecha { get; init; }

        public FechaSeguimiento(DateTime fecha)
        {
            Fecha = fecha;
            Validar();
        }

        public FechaSeguimiento()
        {

        }

        public void Validar()
        {
            if (Fecha == default)
            {
                throw new ArgumentException("La fecha no puede ser nula o inválida");
            }

            if (Fecha > DateTime.Now)
            {
                throw new ArgumentException("La fecha no puede ser futura");
            }

            if (Fecha < DateTime.Now.AddYears(-1))
            {
                throw new ArgumentException("La fecha no puede ser más antigua de un año");
            }
        }
    }
}
