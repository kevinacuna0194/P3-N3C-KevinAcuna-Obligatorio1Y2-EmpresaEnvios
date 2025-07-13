using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class ActualizarEstado : IActualizarEstado
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public ActualizarEstado(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public void Actualizar(EnvioDTO envioDTO, int idUsuarioLogueado, string comentario)
        {
            if (envioDTO == null)
            {
                throw new DatosInvalidosException("El objeto EnvioDTO no puede ser nulo.");
            }

            if (envioDTO.Id <= 0)
            {
                throw new DatosInvalidosException("El ID del envío debe ser mayor que cero.");
            }

            Envio envio = MapeadorEnvio.MapearEnvio(envioDTO);

            if (envio == null)
            {
                throw new DatosInvalidosException("Error al mapear el objeto EnvioDTO a Envio.");
            }

            RepositorioEnvio.ActualizarEstado(envio, idUsuarioLogueado, comentario);
        }
    }
}
