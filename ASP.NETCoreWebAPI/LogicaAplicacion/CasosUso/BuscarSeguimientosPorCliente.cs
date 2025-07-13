using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarSeguimientosPorCliente : IBuscarSeguimientosPorCliente
    {
        public IRepositorioSeguimiento RepositorioSeguimiento { get; set; }

        public BuscarSeguimientosPorCliente(IRepositorioSeguimiento repositorioSeguimiento)
        {
            RepositorioSeguimiento = repositorioSeguimiento ?? throw new ArgumentNullException(nameof(repositorioSeguimiento));
        }

        public IEnumerable<SeguimientoDTO> Buscar(int clienteId)
        {
            if (clienteId <= 0)
            {
                throw new ArgumentException("El ID del cliente debe ser un número positivo.", nameof(clienteId));
            }

            IEnumerable<Seguimiento> seguimientos = RepositorioSeguimiento.ObtenerSeguimientosPorCliente(clienteId);

            if (seguimientos == null || !seguimientos.Any())
            {
                throw new ArgumentNullException("No se encontraron seguimientos para el cliente especificado.", nameof(seguimientos));
            }

            IEnumerable<SeguimientoDTO> seguimientosDTO = MapeadorSeguimiento.MapearListaSeguimientoDTO(seguimientos);

            if (seguimientosDTO == null || !seguimientosDTO.Any())
            {
                throw new ArgumentNullException("No se pudieron mapear los seguimientos a DTOs.", nameof(seguimientosDTO));
            }

            return seguimientosDTO;
        }
    }
}
