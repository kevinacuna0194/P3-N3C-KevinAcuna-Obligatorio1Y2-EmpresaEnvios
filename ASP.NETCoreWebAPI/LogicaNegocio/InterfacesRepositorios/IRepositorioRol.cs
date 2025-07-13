using LogicaNegocio.EntidadesDominio;

namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioRol : IRepositorio<Rol>
    {
        // Obtiene un rol por su nombre.
        Rol ObtenerRolPorNombre(string nombre);

        string ObtenerNombreRolPorId(int id);

        // Verifica si un rol existe por su nombre.
        bool Existe(string nombre);
    }
}
