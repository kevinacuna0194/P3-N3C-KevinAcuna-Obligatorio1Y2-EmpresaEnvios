using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record FechaRegistroUsuario
    {
        public DateTime FechaRegistro { get; init; }

        public FechaRegistroUsuario(DateTime fechaRegistro)
        {
            FechaRegistro = fechaRegistro;
            Validar();
        }

        public FechaRegistroUsuario()
        {
        }

        public void Validar()
        {
            if (FechaRegistro == DateTime.MinValue)
            {
                throw new DatosInvalidosException("La fecha de registro es obligatoria");
            }

            if (FechaRegistro > DateTime.Now)
            {
                throw new DatosInvalidosException("La fecha de registro no puede ser mayor a la fecha actual");
            }

            if (FechaRegistro < DateTime.Now.AddYears(-120))
            {
                throw new DatosInvalidosException("La fecha de registro no puede ser menor a 120 años");
            }
        }
    }
}
