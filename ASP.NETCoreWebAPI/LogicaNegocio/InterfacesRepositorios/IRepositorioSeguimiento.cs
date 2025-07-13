using LogicaNegocio.EntidadesDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioSeguimiento : IRepositorio<Seguimiento>
    {
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvio(int idEnvio);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorCliente(int clienteId);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYCliente(int idEnvio, int clienteId);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorFechaYEnvioYCliente(DateTime fechaInicio, DateTime fechaFin, int idEnvio, int clienteId);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorFechaYEnvio(DateTime fechaInicio, DateTime fechaFin, int idEnvio);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEmpleado(int empleadoId);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorFecha(DateTime fechaInicio, DateTime fechaFin);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYEmpleado(int idEnvio, int empleadoId);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYFecha(int idEnvio, DateTime fechaInicio, DateTime fechaFin);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEmpleadoYFecha(int empleadoId, DateTime fechaInicio, DateTime fechaFin);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioEmpleadoYFecha(int idEnvio, int empleadoId, DateTime fechaInicio, DateTime fechaFin);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorComentario(string comentario);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYComentario(int idEnvio, string comentario);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEmpleadoYComentario(int empleadoId, string comentario);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorFechaYComentario(DateTime fechaInicio, DateTime fechaFin, string comentario);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioEmpleadoFechaYComentario(int idEnvio, int empleadoId, DateTime fechaInicio, DateTime fechaFin, string comentario);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYEmpleadoYComentario(int idEnvio, int empleadoId, string comentario);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEmpleadoYFechaYComentario(int empleadoId, DateTime fechaInicio, DateTime fechaFin, string comentario);
        IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYFechaYComentario(int idEnvio, DateTime fechaInicio, DateTime fechaFin, string comentario);
    }
}
