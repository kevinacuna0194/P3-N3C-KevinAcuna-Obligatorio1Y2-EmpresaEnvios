using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record DetalleAuditoria
    {
        public string Detalle { get; set; }

        public DetalleAuditoria(string detalle)
        {
            Detalle = detalle;
            Validar();
        }

        public DetalleAuditoria()
        {

        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Detalle))
            {
                throw new DatosInvalidosException("El detalle de la auditoría no puede estar vacío");
            }

            if (Detalle.Length > 500)
            {
                throw new DatosInvalidosException("El detalle de la auditoría no puede exceder los 500 caracteres");
            }

            if (Detalle.Length < 10)
            {
                throw new DatosInvalidosException("El detalle de la auditoría debe tener al menos 10 caracteres");
            }
        }
    }
}
