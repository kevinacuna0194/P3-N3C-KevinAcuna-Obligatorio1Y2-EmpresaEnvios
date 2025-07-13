using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record NumeroTrackingEnvio
    {
        public string NumeroTracking { get; init; }
        private static Random random = new Random();


        public NumeroTrackingEnvio(string numeroTracking)
        {
            NumeroTracking = numeroTracking;
            Validar();
        }

        public NumeroTrackingEnvio()
        {

        }

        public void Validar()
        {
            if (String.IsNullOrEmpty(NumeroTracking))
            {
                throw new DatosInvalidosException("El Numero de Tracking es obligatorio");
            }

            if (NumeroTracking.Length < 5)
            {
                throw new DatosInvalidosException("El Numero de Tracking debe tener al menos 5 dígitos");
            }

            if (NumeroTracking.Length > 10)
            {
                throw new DatosInvalidosException("El Numero de Tracking no puede tener más de 10 dígitos");
            }

            if (NumeroTracking.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new DatosInvalidosException("El Numero de Tracking solo puede contener números y letras");
            }

            if (NumeroTracking.Any(char.IsWhiteSpace))
            {
                throw new DatosInvalidosException("El Numero de Tracking no puede contener espacios en blanco");
            }

            if (NumeroTracking.Any(c => char.IsSeparator(c)))
            {
                throw new DatosInvalidosException("El Numero de Tracking no puede contener separadores");
            }

            if (NumeroTracking.Any(c => char.IsControl(c)))
            {
                throw new DatosInvalidosException("El Numero de Tracking no puede contener caracteres de control");
            }

            if (NumeroTracking.Any(c => char.IsSymbol(c)))
            {
                throw new DatosInvalidosException("El Numero de Tracking no puede contener símbolos");
            }

            if (NumeroTracking.Any(c => char.IsPunctuation(c)))
            {
                throw new DatosInvalidosException("El Numero de Tracking no puede contener signos de puntuación");
            }

            if (NumeroTracking.Any(c => char.IsSurrogate(c)))
            {
                throw new DatosInvalidosException("El Numero de Tracking no puede contener caracteres sustitutos");
            }

            if (NumeroTracking.Any(c => char.IsHighSurrogate(c)))
            {
                throw new DatosInvalidosException("El Numero de Tracking no puede contener caracteres sustitutos altos");
            }

            if (NumeroTracking.Any(c => char.IsLowSurrogate(c)))
            {
                throw new DatosInvalidosException("El Numero de Tracking no puede contener caracteres sustitutos bajos");
            }
        }
        public static string GenerarNumeroTracking()
        {
            const string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return "UY" + new string(Enumerable.Range(0, 8)
                .Select(s => caracteresPermitidos[random.Next(caracteresPermitidos.Length)])
                .ToArray());
        }
    }
}
