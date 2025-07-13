using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record FechaEntregaEnvio
    {
        public DateTime FechaEntrega { get; init; }

        public FechaEntregaEnvio(DateTime fechaEntrega)
        {
            FechaEntrega = fechaEntrega;
            Validar();
        }

        public FechaEntregaEnvio()
        {

        }

        public void Validar()
        {
            if (FechaEntrega == default)
            {
                throw new DatosInvalidosException("La fecha de entrega no puede ser nula o vacía");
            }

            //if (FechaEntrega < DateTime.Now.Date)
            //{
            //    throw new DatosInvalidosException("La fecha de entrega no puede ser anterior a la fecha actual");
            //}

            if (FechaEntrega > DateTime.Now.AddDays(30))
            {
                throw new DatosInvalidosException("La fecha de entrega no puede ser más de 30 días en el futuro");
            }

            //if (FechaEntrega.DayOfWeek == DayOfWeek.Saturday || FechaEntrega.DayOfWeek == DayOfWeek.Sunday)
            //{
            //    throw new DatosInvalidosException("La fecha de entrega no puede ser un fin de semana");
            //}

            //if (FechaEntrega.Hour < 8 || FechaEntrega.Hour > 18)
            //{
            //    throw new DatosInvalidosException("La fecha de entrega debe estar dentro del horario laboral (8:00 - 18:00)");
            //}
        }
    }
}
