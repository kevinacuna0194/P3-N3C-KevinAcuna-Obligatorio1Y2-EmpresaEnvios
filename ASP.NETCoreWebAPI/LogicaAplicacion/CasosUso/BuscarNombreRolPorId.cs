using CasosUso.InterfacesCasosUso;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarNombreRolPorId : IBuscarNombreRolPorId
    {
        public IRepositorioRol RepositorioRol { get; set; }

        public BuscarNombreRolPorId(IRepositorioRol repositorioRol)
        {
            RepositorioRol = repositorioRol;
        }
        public string Buscar(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del rol no puede ser menor o igual a cero");
            }

            // 
            string nombreRol = RepositorioRol.ObtenerNombreRolPorId(id);

            if (string.IsNullOrEmpty(nombreRol))
            {
                throw new ArgumentException($"No se encontró un rol con el ID: {id}");
            }

            // Se devuelve el nombre del rol.
            return nombreRol;
        }
    }
}
