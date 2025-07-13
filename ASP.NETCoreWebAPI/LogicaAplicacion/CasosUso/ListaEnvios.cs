using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class ListaEnvios : IListaEnvios
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public ListaEnvios(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public IEnumerable<EnvioDTO> ObtenerListaEnvios()
        {
            IEnumerable<Envio> envios = RepositorioEnvio.ObtenerEnviosComunes();

            if (envios is null || !envios.Any())
            {
                throw new Exception("No se encontraron envíos");
            }

            IEnumerable<EnvioDTO> enviosDTO = MapeadorEnvio.MapearListaEnviosDTO(envios);

            if (enviosDTO == null || !enviosDTO.Any())
            {
                throw new Exception("No se pudo mapear la lista de envíos a DTOs");
            }

            return enviosDTO;
        }
    }
}