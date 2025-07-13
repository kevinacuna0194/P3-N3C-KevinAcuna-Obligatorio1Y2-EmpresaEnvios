using LogicaNegocio.EntidadesDominio;

namespace LogicaNegocio.InterfacesRepositorios
{
    public interface IRepositorioAgencia : IRepositorio<Agencia>
    {
        Agencia ObtenerAgenciaPorNombre(string nombre);        
        Agencia ObtenerAgenciaPorDireccion(string direccion);
        Agencia ObtenerAgenciaPorLatitud(double latitud);
        Agencia ObtenerAgenciaPorLongitud(double longitud);
    }
}
