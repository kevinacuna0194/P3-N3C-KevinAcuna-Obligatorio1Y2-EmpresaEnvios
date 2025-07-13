using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record FechaSalidaEnvio
    {
        public DateTime FechaSalida { get; init; }

        public FechaSalidaEnvio(DateTime fechaSalida)
        {
            FechaSalida = fechaSalida;
            Validar();
        }

        public FechaSalidaEnvio()
        {

        }

        public void Validar()
        {
            if (FechaSalida == default)
            {
                throw new DatosInvalidosException("La fecha de salida no puede ser nula o vacía");
            }

            //if (FechaSalida < DateTime.Now.Date)
            //{
            //    throw new DatosInvalidosException("La fecha de salida no puede ser anterior a la fecha actual");
            //}

            if (FechaSalida > DateTime.Now.AddDays(30))
            {
                throw new DatosInvalidosException("La fecha de salida no puede ser más de 30 días en el futuro");
            }

            //if (FechaSalida.DayOfWeek == DayOfWeek.Saturday || FechaSalida.DayOfWeek == DayOfWeek.Sunday)
            //{
            //    throw new DatosInvalidosException("La fecha de salida no puede ser un fin de semana");
            //}

            //if (FechaSalida.Hour < 8 || FechaSalida.Hour > 18)
            //{
            //    throw new DatosInvalidosException("La fecha de salida debe estar dentro del horario laboral (8:00 - 18:00)");
            //}
        }
    }
}
