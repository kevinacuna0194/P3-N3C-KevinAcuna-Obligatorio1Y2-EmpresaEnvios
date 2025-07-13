using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record LatitudAgencia
    {
        public double Latitud { get; init; }

        public LatitudAgencia(double latitud)
        {
            Latitud = latitud;
            Validar();
        }

        public LatitudAgencia()
        {
        }

        public void Validar()
        {
            if (Latitud < -90 || Latitud > 90)
            {
                throw new DatosInvalidosException("La latitud debe estar entre -90 y 90 grados");
            }

            if (Latitud.ToString().Length > 10)
            {
                throw new DatosInvalidosException("La latitud no puede tener más de 10 caracteres");
            }

            if (Latitud.ToString().Any(c => !char.IsDigit(c) && c != '.' && c != '-'))
            {
                throw new DatosInvalidosException("La latitud solo puede contener dígitos, un punto decimal y un signo negativo");
            }

            if (Latitud.ToString().Any(char.IsControl))
            {
                throw new DatosInvalidosException("La latitud no puede contener caracteres de control");
            }

            if (Latitud.ToString().Count(c => c == '.') > 1)
            {
                throw new DatosInvalidosException("La latitud no puede contener más de un punto decimal");
            }

            if (Latitud.ToString().Count(c => c == '-') > 1)
            {
                throw new DatosInvalidosException("La latitud no puede contener más de un signo negativo");
            }
        }
    }
}
