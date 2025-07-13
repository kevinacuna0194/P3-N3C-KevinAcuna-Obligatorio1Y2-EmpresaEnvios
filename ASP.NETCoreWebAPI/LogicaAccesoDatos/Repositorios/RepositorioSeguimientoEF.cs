using LogicaAccesoDatos.EntityFramework;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioSeguimientoEF : IRepositorioSeguimiento
    {
        public LibraryContext LibraryContext { get; set; }
        public RepositorioSeguimientoEF(LibraryContext libraryContext)
        {
            LibraryContext = libraryContext;
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvio(int idEnvio)
        {
            return LibraryContext.Seguimientos
                .Where(s => s.Envio.Id == idEnvio)
                .Include(S => S.Empleado)
                .AsNoTracking()
                .ToList();
        }

        public void Add(Seguimiento entidad)
        {
            throw new NotImplementedException();
        }

        public void Update(Seguimiento entidad)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Seguimiento FindById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Seguimiento> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorCliente(int clienteId)
        {
            if (clienteId <= 0)
            {
                throw new ArgumentException("El ID del cliente debe ser un número positivo.", nameof(clienteId));
            }

            return LibraryContext.Seguimientos
                .Where(s => s.Envio.Cliente.Id == clienteId)
                .Include(S => S.Empleado)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYCliente(int idEnvio, int clienteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorFechaYEnvioYCliente(DateTime fechaInicio, DateTime fechaFin, int idEnvio, int clienteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorFechaYEnvio(DateTime fechaInicio, DateTime fechaFin, int idEnvio)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEmpleado(int empleadoId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYEmpleado(int idEnvio, int empleadoId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYFecha(int idEnvio, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEmpleadoYFecha(int empleadoId, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioEmpleadoYFecha(int idEnvio, int empleadoId, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorComentario(string comentario)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYComentario(int idEnvio, string comentario)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEmpleadoYComentario(int empleadoId, string comentario)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorFechaYComentario(DateTime fechaInicio, DateTime fechaFin, string comentario)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioEmpleadoFechaYComentario(int idEnvio, int empleadoId, DateTime fechaInicio, DateTime fechaFin, string comentario)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYEmpleadoYComentario(int idEnvio, int empleadoId, string comentario)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEmpleadoYFechaYComentario(int empleadoId, DateTime fechaInicio, DateTime fechaFin, string comentario)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seguimiento> ObtenerSeguimientosPorEnvioYFechaYComentario(int idEnvio, DateTime fechaInicio, DateTime fechaFin, string comentario)
        {
            throw new NotImplementedException();
        }
    }
}
