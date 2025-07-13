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
    public class BuscarSeguimientosPorEnvio : IBuscarSeguimientosPorEnvio
    {
        public IRepositorioSeguimiento RepositorioSeguimiento { get; set; }

        public BuscarSeguimientosPorEnvio(IRepositorioSeguimiento repositorioSeguimiento)
        {
            RepositorioSeguimiento = repositorioSeguimiento;
        }

        public IEnumerable<SeguimientoDTO> Buscar(int idEnvio)
        {
            if (idEnvio <= 0)
            {
                throw new ArgumentException("El ID del envío debe ser mayor que cero.", nameof(idEnvio));
            }

            IEnumerable<Seguimiento> seguimientos = RepositorioSeguimiento.ObtenerSeguimientosPorEnvio(idEnvio);

            if (seguimientos == null || !seguimientos.Any())
            {
                return Enumerable.Empty<SeguimientoDTO>();
            }

            IEnumerable<SeguimientoDTO> seguimientosDTO = MapeadorSeguimiento.MapearListaSeguimientoDTO(seguimientos);

            if (seguimientosDTO == null || !seguimientosDTO.Any())
            {
                return Enumerable.Empty<SeguimientoDTO>();
            }

            return seguimientosDTO;
        }
    }
}
