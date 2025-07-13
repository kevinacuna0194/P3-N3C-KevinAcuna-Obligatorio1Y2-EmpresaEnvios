using ExcepcionesPropias;
using LogicaAccesoDatos.EntityFramework;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioRolEF : IRepositorioRol
    {
        // Propiedad para acceder al contexto de la base de datos
        public LibraryContext LibraryContext { get; set; }

        public RepositorioRolEF(LibraryContext libraryContext)
        {
            LibraryContext = libraryContext;
        }

        public void Add(Rol entity)
        {
            throw new NotImplementedException();
        }

        public bool Existe(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new DatosInvalidosException("El nombre no puede ser nulo o vacío");
            }

            // Verifica si ya existe un rol con el mismo nombre. Si no existe, devuelve false
            return LibraryContext.Roles.Any(rol => rol.Nombre.Nombre == nombre);
        }

        public List<Rol> FindAll()
        {
            return LibraryContext.Roles.ToList();
        }

        public Rol FindById(int id)
        {
            if (id <= 0)
            {
                throw new DatosInvalidosException("El ID del rol no puede ser menor o igual a cero");
            }

            return LibraryContext.Roles.Find(id);
        }

        public Rol ObtenerRolPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new DatosInvalidosException("El nombre no puede ser nulo o vacío");
            }

            return LibraryContext.Roles.Where(rol => rol.Nombre.Nombre == nombre).FirstOrDefault();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Rol entity)
        {
            throw new NotImplementedException();
        }

        public string ObtenerNombreRolPorId(int id)
        {
            if (id <= 0)
            {
                throw new DatosInvalidosException("El ID del rol no puede ser menor o igual a cero");
            }

            string nombreRol = LibraryContext.Roles.Where(rol => rol.Id == id).Select(rol => rol.Nombre.Nombre).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                throw new DatosInvalidosException($"No se encontró un rol con el ID: {id}");
            }

            return nombreRol;
        }
    }
}
