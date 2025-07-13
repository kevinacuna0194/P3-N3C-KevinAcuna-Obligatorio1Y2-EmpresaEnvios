using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class EliminarUsuario : IEliminarUsuario
    {
        public IRepositorioUsuario RepositorioUsuario { get; set; }

        public EliminarUsuario(IRepositorioUsuario repositorioUsuario)
        {
            RepositorioUsuario = repositorioUsuario;
        }
        public void Eliminar(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario no puede ser menor o igual a cero.");
            }

            RepositorioUsuario.Remove(idUsuario);
        }

        public void Eliminar(int id, int idUsuarioLogueado)
        {
            if (id <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario no puede ser menor o igual a cero.");
            }

            if (idUsuarioLogueado <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario logueado no puede ser menor o igual a cero.");
            }

            RepositorioUsuario.Remove(id, idUsuarioLogueado);
        }
    }
}
