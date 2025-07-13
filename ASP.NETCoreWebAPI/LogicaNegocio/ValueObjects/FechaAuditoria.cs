using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record FechaAuditoria
    {
        public DateTime Fecha { get; init; }

        public FechaAuditoria(DateTime fecha)
        {
            Fecha = fecha;
            Validar();
        }

        public FechaAuditoria()
        {

        }

        public void Validar()
        {
            if (Fecha == default)
            {
                throw new DatosInvalidosException("La fecha es obligatoria");
            }

            if (Fecha > DateTime.Now)
            {
                throw new DatosInvalidosException("La fecha no puede ser mayor a la fecha actual");
            }

            if (Fecha < new DateTime(1900, 1, 1))
            {
                throw new DatosInvalidosException("La fecha no puede ser menor a 1900");
            }

            if (Fecha > new DateTime(2100, 1, 1))
            {
                throw new DatosInvalidosException("La fecha no puede ser mayor a 2100");
            }
        }
    }
}
