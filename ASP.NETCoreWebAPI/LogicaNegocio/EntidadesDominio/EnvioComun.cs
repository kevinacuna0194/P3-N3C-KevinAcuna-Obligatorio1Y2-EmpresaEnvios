using Enum;
using ExcepcionesPropias;
using LogicaNegocio.ValueObjects;

namespace LogicaNegocio.EntidadesDominio
{
    public class EnvioComun : Envio
    {
        // Propiedades específicas de EnvioComun
        public Agencia Agencia { get; set; }

        public EnvioComun() { }

        public EnvioComun(NumeroTrackingEnvio numeroTracking, Usuario empleado, Usuario cliente, PesoEnvio peso, FechaSalidaEnvio fechaSalida, FechaEntregaEnvio fechaEntrega, EstadoEnvio estado, Agencia agenciaDestino) : base(numeroTracking, empleado, cliente, peso, fechaSalida, fechaEntrega, estado)
        {
            Agencia = agenciaDestino;
        }

        public override void Validar()
        {
            base.Validar();

            if (Agencia is null)
            {
                throw new DatosInvalidosException("La Agencia de destino es obligatoria");
            }
        }

        public override void FinalizarEnvio()
        {
            // Implementación específica para finalizar un envío común
            // Aquí puedes agregar la lógica necesaria para finalizar el envío común
            // Por ejemplo, actualizar el estado del envío, registrar la entrega, etc.
            base.FinalizarEnvio();
        }
    }
}
