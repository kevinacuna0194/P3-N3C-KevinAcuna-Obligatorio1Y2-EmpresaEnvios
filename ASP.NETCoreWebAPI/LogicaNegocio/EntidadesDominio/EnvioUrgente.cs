using Enum;
using LogicaNegocio.ValueObjects;

namespace LogicaNegocio.EntidadesDominio
{
    public class EnvioUrgente : Envio
    {
        // Propiedades específicas de EnvioUrgente
        public DireccionPostalEnvioUrgente DireccionPostal { get; set; }
        public bool EntregaEficiente { get; set; }

        public EnvioUrgente() { }

        // Constructor para inicializar las propiedades específicas
        public EnvioUrgente(NumeroTrackingEnvio numeroTracking, Usuario empleado, Usuario cliente, PesoEnvio peso, FechaSalidaEnvio fechaSalida, FechaEntregaEnvio fechaEntrega, EstadoEnvio estado, DireccionPostalEnvioUrgente direccionPostal, bool entregaEficiente) : base(numeroTracking, empleado, cliente, peso, fechaSalida, fechaEntrega, estado)
        {
            DireccionPostal = direccionPostal;
            EntregaEficiente = entregaEficiente;
        }

        public virtual void Validar()
        {
            base.Validar();
        }
        public override void FinalizarEnvio()
        {
            // Implementación específica para finalizar un envío urgente
            // Aquí puedes agregar la lógica necesaria para finalizar el envío urgente
            // Por ejemplo, actualizar el estado del envío, registrar la entrega, etc.
            base.FinalizarEnvio();
        }

        public void CalcularEntregaEficiente()
        {
            if (FechaSalida != null && FechaEntrega != null)
            {
                // Calcular la diferencia entre las fechas de salida y entrega
                TimeSpan diferencia = FechaEntrega.FechaEntrega - FechaSalida.FechaSalida;

                // Verificar si la diferencia es menor a 24 horas
                EntregaEficiente = diferencia.TotalHours < 24;
            }
            else
            {
                EntregaEficiente = false;
            }
        }
    }
}
