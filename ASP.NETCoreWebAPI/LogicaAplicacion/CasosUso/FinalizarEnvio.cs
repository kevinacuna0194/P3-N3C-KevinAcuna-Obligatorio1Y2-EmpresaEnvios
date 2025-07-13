using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class FinalizarEnvio : IFinalizarEnvio
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }
        public FinalizarEnvio(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public void Finalizar(EnvioDTO envioDTO, int idUsuarioLogueado)
        {
            if (envioDTO == null)
            {
                throw new DatosInvalidosException("El objeto EnvioDTO no puede ser nulo.");
            }

            if (idUsuarioLogueado <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario logueado debe ser mayor que cero.");
            }

            Envio envio = MapeadorEnvio.MapearEnvio(envioDTO);

            if (envio == null)
            {
                throw new DatosInvalidosException("Error al mapear el objeto EnvioDTO a Envio.");
            }

            // Llamar al método de finalización del envío en el repositorio
            RepositorioEnvio.FinalizarEnvio(envio, idUsuarioLogueado);
        }
    }
}
