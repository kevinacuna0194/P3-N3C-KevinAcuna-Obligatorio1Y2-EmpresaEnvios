using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarEnvioPorNumeroTracking : IBuscarEnvioPorNumeroTracking
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }
        public BuscarEnvioPorNumeroTracking(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public EnvioDTO Buscar(string numeroTracking)
        {
            if (string.IsNullOrEmpty(numeroTracking))
            {
                throw new ArgumentException("El número de tracking no puede ser nulo o vacío.");
            }

            Envio envio = RepositorioEnvio.ObtenerEnvioPorNumeroTracking(numeroTracking);

            if (envio == null)
            {
                throw new ArgumentException("No se encontró un envío con el número de tracking proporcionado.");
            }

            EnvioDTO envioDTO = MapeadorEnvio.MapearEnvioDTO(envio);

            if (envioDTO == null)
            {
                throw new ArgumentException("Error al mapear envio a envioDTO");
            }

            return envioDTO;
        }
    }
}
