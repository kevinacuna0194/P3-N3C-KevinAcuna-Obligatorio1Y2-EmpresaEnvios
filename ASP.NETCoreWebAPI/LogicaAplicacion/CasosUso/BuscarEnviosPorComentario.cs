using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarEnviosPorComentario : IBuscarEnviosPorComentario
    {
        public IRepositorioEnvio RepositorioEnvio { get; set; }

        public BuscarEnviosPorComentario(IRepositorioEnvio repositorioEnvio)
        {
            RepositorioEnvio = repositorioEnvio;
        }

        public IEnumerable<EnvioDTO> Buscar(string comentario, int idCliente)
        {
            if (string.IsNullOrEmpty(comentario))
            {
                throw new ArgumentNullException(nameof(comentario), "El comentario no puede ser nulo ni estar vacío.");
            }

            if (idCliente <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(idCliente), "El id del cliente no puede ser menor o igual a cero.");
            }

            IEnumerable<Envio> envios = RepositorioEnvio.ObtenerEnviosPorComentario(comentario, idCliente);

            if (envios is null || !envios.Any())
            {
                throw new ArgumentNullException(nameof(envios), "No se encontraron envíos que coincidan con el comentario proporcionado.");
            }

            IEnumerable<EnvioDTO> enviosDTO = MapeadorEnvio.MapearListaEnviosDTO(envios);

            if (enviosDTO is null || !enviosDTO.Any())
            {
                throw new ArgumentNullException(nameof(enviosDTO), "La lista de envíos mapeada está vacía o no se pudo generar correctamente.");
            }

            return enviosDTO;
        }
    }
}