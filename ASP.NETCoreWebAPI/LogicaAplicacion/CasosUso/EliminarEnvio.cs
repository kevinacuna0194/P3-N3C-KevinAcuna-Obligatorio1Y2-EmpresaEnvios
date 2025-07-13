using CasosUso.InterfacesCasosUso;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class EliminarEnvio : IEliminarEnvio
    {
        public IRepositorioEnvio ReposistorioEnvio { get; set; }
        public EliminarEnvio(IRepositorioEnvio repositorioEnvio)
        {
            repositorioEnvio = repositorioEnvio;
        }

        public void Eliminar(int id, int idUsuarioLogueado)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del envío debe ser mayor que cero.");
            }

            ReposistorioEnvio.Remove(id, idUsuarioLogueado);
        }
    }
}
