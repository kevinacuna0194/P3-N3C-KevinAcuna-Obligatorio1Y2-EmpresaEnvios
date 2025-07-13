using LogicaAccesoDatos.EntityFramework;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioAgenciaEF : IRepositorioAgencia
    {
        public LibraryContext LibraryContext { get; set; }

        public RepositorioAgenciaEF(LibraryContext libraryContext)
        {
            LibraryContext = libraryContext;
        }

        public void Add(Agencia entidad)
        {
            throw new NotImplementedException();
        }

        public List<Agencia> FindAll()
        {
            return LibraryContext.Agencias.ToList();
        }

        public Agencia FindById(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException();

            return LibraryContext.Agencias.Find(id) ?? throw new KeyNotFoundException($"Agencia con ID {id} no encontrada.");
        }

        public Agencia ObtenerAgenciaPorDireccion(string direccion)
        {
            throw new NotImplementedException();
        }

        public Agencia ObtenerAgenciaPorLatitud(double latitud)
        {
            throw new NotImplementedException();
        }

        public Agencia ObtenerAgenciaPorLongitud(double longitud)
        {
            throw new NotImplementedException();
        }

        public Agencia ObtenerAgenciaPorNombre(string nombre)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Agencia entidad)
        {
            throw new NotImplementedException();
        }
    }
}
