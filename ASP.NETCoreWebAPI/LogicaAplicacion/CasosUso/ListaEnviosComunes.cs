using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class ListaEnviosComunes : IListaEnviosComunes
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public ListaEnviosComunes(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public IEnumerable<EnvioDTO> ObtenerEnviosComunes()
        {
            IEnumerable<EnvioComun> enviosComunes = RepositorioEnvio.ObtenerEnviosComunes();

            if (enviosComunes is null || !enviosComunes.Any())
            {
                throw new DatosInvalidosException("No se encontraron envíos comunes");
            }

            IEnumerable<EnvioDTO> enviosComunesDTO = MapeadorEnvio.MapearListaEnviosDTO(enviosComunes);

            if (enviosComunesDTO == null || !enviosComunesDTO.Any())
            {
                throw new DatosInvalidosException("No se pudo mapear la lista de envíos comunes a DTOs");
            }

            return enviosComunesDTO;
        }
    }
}
