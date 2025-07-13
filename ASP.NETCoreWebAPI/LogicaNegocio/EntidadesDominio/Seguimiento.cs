using ExcepcionesPropias;
using LogicaNegocio.InterfacesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaNegocio.EntidadesDominio
{
    public class Seguimiento : IValidable
    {
        public int Id { get; set; }
        public Envio Envio { get; set; }
        public FechaSeguimiento Fecha { get; set; }
        public Usuario Empleado { get; set; }
        public ComentarioSeguimiento Comentario { get; set; }

        public Seguimiento() { }

        public Seguimiento(Envio envio, FechaSeguimiento fecha, Usuario empleado, ComentarioSeguimiento comentario)
        {
            Envio = envio;
            Fecha = fecha;
            Empleado = empleado;
            Comentario = comentario;
        }

        public void Validar()
        {
            if (Envio != null)
            {
                Envio.Validar();
            }
            else
            {
                throw new DatosInvalidosException("El envío no puede ser nulo.");
            }

            if (Empleado == null)
            {
                throw new DatosInvalidosException("El empleado es obligatorio.");
            }
        }
    }
}
