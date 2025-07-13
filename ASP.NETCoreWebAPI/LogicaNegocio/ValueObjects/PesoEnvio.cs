using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record PesoEnvio
    {
        public decimal Peso { get; init; }
        private const decimal PesoMinimo = 0.05m;
        private const decimal PesoMaximo = 200000m;


        public PesoEnvio(decimal peso)
        {
            Peso = peso;
            Validar();
        }

        public PesoEnvio()
        {

        }        

        public void Validar()
        {
            if (Peso < PesoMinimo)
            {
                throw new DatosInvalidosException("El peso no puede ser menor a 0.05 kg (50g)");
            }

            if (Peso > PesoMaximo)
            {
                throw new DatosInvalidosException("El peso no puede ser mayor a 200 kg");
            }

            if (Math.Round(Peso, 2) != Peso)
            {
                throw new DatosInvalidosException("El peso debe tener como máximo 2 decimales");
            }
        }
    }
}
