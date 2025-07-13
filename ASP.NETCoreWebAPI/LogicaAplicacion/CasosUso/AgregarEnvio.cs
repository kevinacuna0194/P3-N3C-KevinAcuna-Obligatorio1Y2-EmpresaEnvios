using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class AgregarEnvio : IAgregarEnvio
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public AgregarEnvio(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }
        public void Agregar(EnvioDTO envioDTO)
        {
            if (envioDTO is null)
            {
                throw new DatosInvalidosException("El EnvioDTO no puede ser nulo");
            }

            Envio envio = MapeadorEnvio.MapearEnvio(envioDTO);

            envio.Validar();

            RepositorioEnvio.Add(envio);
        }

        public void Agregar(EnvioDTO envioDTO, int idUsuarioLogueado)
        {
            if (envioDTO is null)
            {
                throw new DatosInvalidosException("El EnvioDTO no puede ser nulo");
            }

            if (idUsuarioLogueado <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario logueado no puede ser menor o igual a cero");
            }

            Envio envio = MapeadorEnvio.MapearEnvio(envioDTO);

            envio.Validar();

            RepositorioEnvio.Add(envio, idUsuarioLogueado);
        }
    }
}
