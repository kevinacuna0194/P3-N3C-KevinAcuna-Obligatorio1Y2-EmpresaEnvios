using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using Enum;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarEnviosPorTipo : IBuscarEnviosPorTipo
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public BuscarEnviosPorTipo(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public IEnumerable<EnvioDTO> Buscar(TipoEnvio tipoEnvio)
        {
            IEnumerable<Envio> envios = RepositorioEnvio.ObtenerEnviosPorTipo(tipoEnvio);

            if (envios == null || !envios.Any())
            {
                return Enumerable.Empty<EnvioDTO>();
            }

            IEnumerable<EnvioDTO> enviosDTO = MapeadorEnvio.MapearListaEnviosDTO(envios);

            if (enviosDTO == null || !enviosDTO.Any())
            {
                throw new Exception("Error al mapear la lista de envíos a DTOs.");
            }

            return enviosDTO;
        }
    }
}
