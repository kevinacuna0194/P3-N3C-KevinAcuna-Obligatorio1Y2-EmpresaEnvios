using CasosUso.DTOs;
using ExcepcionesPropias;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.Mapeadores
{
    public class MapeadorSeguimiento
    {
        public static Seguimiento MapearSeguimiento(SeguimientoDTO seguimientoDTO)
        {
            if (seguimientoDTO == null)
            {
                throw new DatosInvalidosException("El objeto SeguimientoDTO no puede ser nulo.");
            }

            return new Seguimiento
            {
                Id = seguimientoDTO.Id,
                Fecha = new FechaSeguimiento(seguimientoDTO.Fecha),
                Empleado = new Usuario { Id = seguimientoDTO.EmpleadoId },
                Comentario = new ComentarioSeguimiento(seguimientoDTO.Comentario)
            };
        }

        public static SeguimientoDTO MapearSeguimientoDTO(Seguimiento seguimiento)
        {
            if (seguimiento == null)
            {
                throw new DatosInvalidosException("El objeto Seguimiento no puede ser nulo.");
            }
            return new SeguimientoDTO
            {
                Id = seguimiento.Id,
                Fecha = seguimiento.Fecha.Fecha,
                EmpleadoId = seguimiento.Empleado.Id,
                NombreEmpleado = seguimiento.Empleado.Nombre.Nombre, // Asumiendo que tienes una propiedad NombreCompleto en Usuario
                Comentario = seguimiento.Comentario.Comentario
            };
        }

        public static IEnumerable<SeguimientoDTO> MapearListaSeguimientoDTO(IEnumerable<Seguimiento> seguimientos)
        {
            if (seguimientos == null || !seguimientos.Any())
            {
                throw new DatosInvalidosException("La lista de seguimientos no puede ser nula o vacía.");
            }

            return seguimientos.Select(MapearSeguimientoDTO).ToList();
        }
    }
}
