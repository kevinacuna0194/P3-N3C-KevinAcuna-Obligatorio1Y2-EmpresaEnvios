using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record AccionAuditoria
    {
        public string Accion { get; init; }

        public AccionAuditoria(string accion)
        {
            Accion = accion;
            Validar();
        }

        public AccionAuditoria()
        {
        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Accion))
            {
                throw new DatosInvalidosException("La acción es obligatoria");
            }

            if (Accion.Length < 5)
            {
                throw new DatosInvalidosException("La acción debe tener al menos 5 caracteres");
            }

            if (Accion.Length > 50)
            {
                throw new DatosInvalidosException("La acción no puede tener más de 50 caracteres");
            }

            if (Accion.Any(char.IsWhiteSpace))
            {
                throw new DatosInvalidosException("La acción no puede contener espacios en blanco");
            }

            if (Accion.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new DatosInvalidosException("La acción solo puede contener letras y números");
            }

            if (Accion.Any(c => char.IsPunctuation(c)))
            {
                throw new DatosInvalidosException("La acción no puede contener caracteres especiales");
            }

            if (Accion.Any(c => char.IsSymbol(c)))
            {
                throw new DatosInvalidosException("La acción no puede contener caracteres especiales");
            }
        }
    }
}
