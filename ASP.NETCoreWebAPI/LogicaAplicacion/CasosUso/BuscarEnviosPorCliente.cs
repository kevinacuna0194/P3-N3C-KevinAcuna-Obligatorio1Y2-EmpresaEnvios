using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarEnviosPorCliente : IBuscarEnviosPorCliente
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public BuscarEnviosPorCliente(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio ?? throw new ArgumentNullException(nameof(repositorioEnvio));
        }

        public IEnumerable<EnvioDTO> Buscar(int clienteId)
        {
            if (clienteId <= 0)
            {
                throw new ArgumentException("El ID del cliente debe ser un número positivo.", nameof(clienteId));
            }

            IEnumerable<Envio> envios = RepositorioEnvio.ObtenerEnviosPorCliente(clienteId);
             
            if (envios == null || !envios.Any())
            {
                throw new ArgumentNullException("No se encontraron envíos para el cliente especificado.", nameof(envios));
            }

            IEnumerable<EnvioDTO> enviosDTO = MapeadorEnvio.MapearListaEnviosDTO(envios);

            if (enviosDTO == null || !enviosDTO.Any())
            {
                throw new ArgumentNullException("No se pudieron mapear los envíos a DTOs.", nameof(enviosDTO));
            }

            return enviosDTO;
        }
    }
}
