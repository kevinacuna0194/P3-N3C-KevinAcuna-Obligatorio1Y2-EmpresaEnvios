using LogicaAplicacion.Mapeadores;
using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using Enum;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarEnviosPorTipoYEstado : IBuscarEnviosPorTipoYEstado
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }
        
        public BuscarEnviosPorTipoYEstado(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public IEnumerable<EnvioDTO> Buscar(TipoEnvio tipoEnvio, EstadoEnvio estadoEnvio)
        {
            IEnumerable<Envio> envios = RepositorioEnvio.ObtenerEnviosPorTipoYEstado(tipoEnvio, estadoEnvio);

            if(envios == null || !envios.Any())
            {
                return Enumerable.Empty<EnvioDTO>();
            }

            IEnumerable<EnvioDTO> enviosDTO = MapeadorEnvio.MapearListaEnviosDTO(envios);

            if (enviosDTO == null || !enviosDTO.Any())
            {
                return Enumerable.Empty<EnvioDTO>();
            }

            return enviosDTO;
        }
    }
}
