using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class ListaAgencias : IListaAgencias
    {
        public IRepositorioAgencia RepositorioAgencia { get; set; }

        public ListaAgencias(IRepositorioAgencia repositorioAgencia)
        {
            RepositorioAgencia = repositorioAgencia;
        }

        public IEnumerable<AgenciaDTO> ObtenerListaAgencias()
        {
            IEnumerable<Agencia> agencias = RepositorioAgencia.FindAll();

            if (agencias is null || !agencias.Any())
            {
                throw new Exception("No se encontraron agencias");
            }

            IEnumerable<AgenciaDTO> agenciasDTO = agencias.Select(agencia => new AgenciaDTO
            {
                Id = agencia.Id,
                Nombre = agencia.Nombre.Nombre,
                DireccionPostal = agencia.DireccionPostal.DireccionPostal,
                Latitud = agencia.Latitud.Latitud,
                Longitud = agencia.Longitud.Longitud
            });

            return agenciasDTO;
        }
    }
}
