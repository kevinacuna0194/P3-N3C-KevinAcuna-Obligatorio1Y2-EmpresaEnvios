using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using Enum;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarEnviosPorEstado : IBuscarEnviosPorEstado
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public BuscarEnviosPorEstado(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public IEnumerable<EnvioDTO> Buscar(EstadoEnvio estado)
        {
            IEnumerable<Envio> envios = RepositorioEnvio.ObtenerEnviosPorEstado(estado);

            if (envios == null || !envios.Any())
            {
                throw new Exception("No se encontraron envíos con el estado especificado.");
            }

            IEnumerable<EnvioDTO> enviosDTO = MapeadorEnvio.MapearListaEnviosDTO(envios);

            if (enviosDTO == null || !enviosDTO.Any())
            {
                throw new Exception("No se encontraron envíos con el estado especificado.");
            }

            return enviosDTO;
        }
    }
}
