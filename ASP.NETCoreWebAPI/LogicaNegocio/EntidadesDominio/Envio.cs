using Enum;
using ExcepcionesPropias;
using LogicaNegocio.InterfacesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaNegocio.EntidadesDominio
{
    public abstract class Envio : IValidable
    {
        // Propiedades comunes a todas las clases de envío
        public int Id { get; set; }
        public NumeroTrackingEnvio NumeroTracking { get; set; }
        public Usuario Empleado { get; set; }
        public Usuario Cliente { get; set; }
        public PesoEnvio Peso { get; set; }
        public FechaSalidaEnvio FechaSalida { get; set; }
        public FechaEntregaEnvio? FechaEntrega { get; set; }
        public EstadoEnvio Estado { get; set; }
        public List<Seguimiento> Seguimientos { get; set; } = new List<Seguimiento>();
         
        public Envio() 
        { 

        }

        // Constructor para inicializar las propiedades comunes
        public Envio(NumeroTrackingEnvio numeroTracking, Usuario empleado, Usuario cliente, PesoEnvio peso, FechaSalidaEnvio fechaSalida, FechaEntregaEnvio fechaEntrega, EstadoEnvio estado)
        {
            NumeroTracking = numeroTracking;
            Empleado = empleado;
            Cliente = cliente;
            Peso = peso;
            FechaSalida = fechaSalida;
            FechaEntrega = fechaEntrega;
            Estado = estado;
        }

        public virtual void FinalizarEnvio()
        {
            if (Estado == EstadoEnvio.Finalizado)
            {
                throw new DatosInvalidosException("El envío ya fue finalizado.");
            }

            Estado = EstadoEnvio.Finalizado;
        }

        public virtual void Validar()
        {
            if (Empleado is null)
            {
                throw new DatosInvalidosException("El Empleado es obligatorio");
            }

            if (Cliente is null)
            {
                throw new DatosInvalidosException("El Cliente es obligatorio");
            }
        }
    }
}
