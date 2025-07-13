using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using Enum;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarEnvioPorId : IBuscarEnvioPorId
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public BuscarEnvioPorId(IRepositorioEnvio respositorioEnvio)
        {
            RepositorioEnvio = respositorioEnvio;
        }

        public EnvioDTO Buscar(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del envío debe ser mayor que cero");
            }

            Envio envio = RepositorioEnvio.FindById(id);

            if (envio == null)
            {
                throw new Exception($"No se encontró el envío con el ID: {id}");
            }

            EnvioDTO envioDTO = MapeadorEnvio.MapearEnvioDTO(envio);

            return envioDTO;
        }

        public EnvioDTO Buscar(int id, TipoEnvio tipoEnvio)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del envío debe ser mayor que cero");
            }

            Envio envio = RepositorioEnvio.FindById(id, tipoEnvio);

            if (envio == null)
            {
                throw new Exception("No se encontró el envío con el ID proporcionado");
            }

            EnvioDTO envioDTO = MapeadorEnvio.MapearEnvioDTO(envio);

            return envioDTO;
        }
    }
}
