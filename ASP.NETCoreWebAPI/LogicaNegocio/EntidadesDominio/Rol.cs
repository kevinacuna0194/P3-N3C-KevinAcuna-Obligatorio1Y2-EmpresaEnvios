using LogicaNegocio.ValueObjects;

namespace LogicaNegocio.EntidadesDominio
{
    public class Rol
    {
        public int Id { get; set; }
        public NombreRol Nombre { get; set; }

        public Rol(NombreRol nombre)
        {
            Nombre = nombre;
        }
        public Rol()
        {
        }
    }
}
