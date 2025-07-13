using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class ListaEnviosUrgentes : IListaEnviosUrgentes
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public ListaEnviosUrgentes(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public IEnumerable<EnvioDTO> ObtenerEnviosUrgentes()
        {
            IEnumerable<EnvioUrgente> envioUrgentes = RepositorioEnvio.ObtenerEnviosUrgentes();

            if (envioUrgentes == null || !envioUrgentes.Any())
            {
                return Enumerable.Empty<EnvioDTO>();
            }

            IEnumerable<EnvioDTO> enviosUrgentesDTO = MapeadorEnvio.MapearListaEnviosDTO(envioUrgentes);

            if (enviosUrgentesDTO == null || !enviosUrgentesDTO.Any())
            {
                return Enumerable.Empty<EnvioDTO>();
            }

            return enviosUrgentesDTO;
        }
    }
}
