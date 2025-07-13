using ExcepcionesPropias;
using LogicaNegocio.InterfacesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaNegocio.EntidadesDominio
{
    public class Auditoria : IValidable
    {
        public int Id { get; set; }
        public AccionAuditoria Accion { get; set; }
        public FechaAuditoria Fecha { get; set; }
        public Usuario Usuario { get; set; }
        public DetalleAuditoria Detalle { get; set; }

        public Auditoria(AccionAuditoria accion, FechaAuditoria fecha, Usuario usuario)
        {
            Accion = accion;
            Fecha = fecha;
            Usuario = usuario;
        }

        /// Constructor sin parámetros
        public Auditoria()
        {

        }

        public void Validar()
        {
            if (Usuario is null)
            {
                throw new DatosInvalidosException("El usuario es obligatorio");
            }
        }
    }
}
