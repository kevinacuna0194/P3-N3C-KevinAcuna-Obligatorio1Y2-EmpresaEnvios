using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record LongitudAgencia
    {
        public double Longitud { get; init; }

        public LongitudAgencia(double latitud)
        {
            Longitud = latitud;
            Validar();
        }

        public LongitudAgencia()
        {
        }

        public void Validar()
        {
            if (Longitud < -180 || Longitud > 180)
            {
                throw new DatosInvalidosException("La longitud debe estar entre -180 y 180 grados");
            }

            if (Longitud.ToString().Length > 10)
            {
                throw new DatosInvalidosException("La longitud no puede tener más de 10 caracteres");
            }

            if (Longitud.ToString().Any(c => !char.IsDigit(c) && c != '.' && c != '-'))
            {
                throw new DatosInvalidosException("La longitud solo puede contener dígitos, un punto decimal y un signo negativo");
            }

            if (Longitud.ToString().Any(char.IsControl))
            {
                throw new DatosInvalidosException("La longitud no puede contener caracteres de control");
            }

            if (Longitud.ToString().Count(c => c == '.') > 1)
            {
                throw new DatosInvalidosException("La longitud no puede contener más de un punto decimal");
            }

            if (Longitud.ToString().Count(c => c == '-') > 1)
            {
                throw new DatosInvalidosException("La longitud no puede contener más de un signo negativo");
            }
        }
    }
}
