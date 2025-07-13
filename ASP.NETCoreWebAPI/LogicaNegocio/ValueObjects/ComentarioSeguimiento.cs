using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record ComentarioSeguimiento
    {
        public string Comentario { get; init; }

        public ComentarioSeguimiento(string comentario)
        {
            Comentario = comentario;
            Validar();
        }

        public ComentarioSeguimiento()
        {

        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Comentario))
            {
                throw new DatosInvalidosException("El comentario no puede estar vacío");
            }

            if (Comentario.Length > 500)
            {
                throw new DatosInvalidosException("El comentario no puede exceder los 500 caracteres");
            }

            if (Comentario.Length < 10)
            {
                throw new DatosInvalidosException("El comentario debe tener al menos 10 caracteres");
            }

            if (Comentario.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new DatosInvalidosException("El comentario contiene caracteres no permitidos");
            }

            if (Comentario.Any(c => char.IsSymbol(c)))
            {
                throw new DatosInvalidosException("El comentario contiene caracteres no permitidos");
            }
        }
    }
}
