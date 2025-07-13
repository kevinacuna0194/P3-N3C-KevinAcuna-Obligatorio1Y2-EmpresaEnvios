using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enum
{
    public enum EstadoEnvio
    {
        Todos,           // Representa todos los estados posibles de un envío.
        EnProcesoEntrega,  // El paquete está en manos del repartidor y pronto será entregado.        
        Finalizado         // El envío ha concluido completamente, sin más acciones pendientes.
    }

    /*
    public enum EstadoEnvio
    {
        Pendiente,         // Se ha generado la orden, pero el envío aún no ha comenzado.
        EnTransito,        // El paquete está en camino hacia su destino.
        EnAduana,          // El envío está siendo revisado por las autoridades aduaneras.
        EnProcesoEntrega,  // El paquete está en manos del repartidor y pronto será entregado.
        Entregado,         // El destinatario ha recibido el paquete.
        Rechazado,         // La entrega no pudo completarse porque el destinatario no la aceptó.
        Retenido,          // El paquete está detenido por algún motivo.
        Extraviado,        // El envío no ha llegado a su destino y se requiere una investigación.
        DevueltoRemitente, // No se pudo completar la entrega y el paquete regresará a su origen.
        Finalizado         // El envío ha concluido completamente, sin más acciones pendientes.
    }
    */

    public static class EstadoEnvioHelper
    {
        public static string ObtenerDescripcionEstado(EstadoEnvio estado)
        {
            return estado switch
            {
                EstadoEnvio.EnProcesoEntrega => "En Proceso de Entrega",
                EstadoEnvio.Finalizado => "Finalizado",
                EstadoEnvio.Todos => "Todos",
                _ => estado.ToString()
            };
        }

        // Obtener los estados de envío disponibles
        public static IEnumerable<EstadoEnvio> ObtenerEstadosEnvio()
        {
            return System.Enum.GetValues(typeof(EstadoEnvio)).Cast<EstadoEnvio>();
        }
    }
}

