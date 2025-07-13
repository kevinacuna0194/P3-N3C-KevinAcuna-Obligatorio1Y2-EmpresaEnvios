using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using Enum;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarEnviosPorEstadoYRangoFechas : IBuscarEnviosPorEstadoYRangoFechas
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public BuscarEnviosPorEstadoYRangoFechas(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public IEnumerable<EnvioDTO> Buscar(EstadoEnvio estado, DateTime fechaInicio, DateTime fechaFin, int idCliente)
        {
            if (fechaInicio > fechaFin)
            {
                throw new ArgumentException("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }

            IEnumerable<Envio> envios = RepositorioEnvio.ObtenerEnviosPorEstadoYRangoFechas(estado, fechaInicio, fechaFin, idCliente);

            if (envios == null || !envios.Any())
            {
                throw new InvalidOperationException("No se encontraron envíos para el estado y rango de fechas especificados.");
            }

            IEnumerable<EnvioDTO> envioDTO = MapeadorEnvio.MapearListaEnviosDTO(envios);

            if (envioDTO == null || !envioDTO.Any())
            {
                throw new InvalidOperationException("No se pudieron mapear los envíos a DTOs.");
            }

            return envioDTO;
        }
    }
}
