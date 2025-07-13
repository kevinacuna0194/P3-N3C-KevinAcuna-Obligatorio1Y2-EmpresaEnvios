using LogicaNegocio.ValueObjects;

namespace LogicaNegocio.EntidadesDominio
{
    public class Agencia
    {
        public int Id { get; set; }
        public NombreAgencia Nombre { get; set; }
        public DireccionPostalAgencia DireccionPostal { get; set; }
        public LatitudAgencia Latitud { get; set; }
        public LongitudAgencia Longitud { get; set; }

        public Agencia() { }

        public Agencia(NombreAgencia nombre, DireccionPostalAgencia direccionPostal, LatitudAgencia Latitud, LongitudAgencia Longitud)
        {
            Nombre = nombre;
            DireccionPostal = direccionPostal;
            Latitud = Latitud;
            Longitud = Longitud;
        }
    }
}
