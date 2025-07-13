using CasosUso.DTOs;
using ExcepcionesPropias;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaAplicacion.Mapeadores
{
    public class MapeadorAuditoria
    {
        public static Auditoria MapearAuditoria(AuditoriaDTO auditoriaDTO)
        {
            if (auditoriaDTO == null)
            {
                throw new DatosInvalidosException("El objeto AuditoriaDTO no puede ser nulo.");
            }

            Auditoria auditoria = new Auditoria();

            auditoria.Id = auditoriaDTO.Id;
            auditoria.Accion = new AccionAuditoria { Accion = auditoriaDTO.Accion };
            auditoria.Fecha = new FechaAuditoria { Fecha = auditoriaDTO.Fecha };
            auditoria.Usuario = new Usuario { Id = auditoriaDTO.UsuarioId, Nombre = new NombreUsuario(auditoriaDTO.NombreUsuario) };
            auditoria.Detalle = new DetalleAuditoria { Detalle = auditoriaDTO.Detalle };

            return auditoria;
        }

        public static AuditoriaDTO MapearAuditoriaDTO(Auditoria auditoria)
        {
            if (auditoria is null)
            {
                throw new DatosInvalidosException("El objeto Auditoria no puede ser nulo.");
            }

            AuditoriaDTO auditoriaDTO = new AuditoriaDTO();

            auditoriaDTO.Id = auditoria.Id;
            auditoriaDTO.Accion = auditoria.Accion.Accion;
            auditoriaDTO.Fecha = auditoria.Fecha.Fecha;
            auditoriaDTO.UsuarioId = auditoria.Usuario.Id;
            auditoriaDTO.NombreUsuario = auditoria.Usuario.Nombre.Nombre;
            auditoriaDTO.Detalle = auditoria.Detalle.Detalle;

            return auditoriaDTO;
        }

        public static List<AuditoriaDTO> MapearListaAuditoriaDTO(List<Auditoria> auditorias)
        {

            if (auditorias.Count == 0)
            {
                throw new DatosInvalidosException("La lista de auditorias no puede estar vacía.");
            }

            if (auditorias is null)
            {
                throw new DatosInvalidosException("La lista de auditorias no puede ser nula.");
            }

            List<AuditoriaDTO> auditoriaDTOs = new List<AuditoriaDTO>();

            foreach (var auditoria in auditorias)
            {
                auditoriaDTOs.Add(MapearAuditoriaDTO(auditoria));
            }

            return auditoriaDTOs;
        }
    }
}
