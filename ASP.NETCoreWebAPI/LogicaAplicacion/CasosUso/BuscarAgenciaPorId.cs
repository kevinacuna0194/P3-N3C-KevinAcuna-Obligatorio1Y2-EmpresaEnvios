using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using LogicaAplicacion.Mapeadores;
using LogicaNegocio.InterfacesRepositorios;

namespace LogicaAplicacion.CasosUso
{
    public class BuscarAgenciaPorId : IBuscarAgenciaPorId
    {
        public IRepositorioAgencia RepositorioAgencia { get; set; }

        public BuscarAgenciaPorId(IRepositorioAgencia repositorioAgencia)
        {
            RepositorioAgencia = repositorioAgencia;
        }

        public AgenciaDTO Buscar(int idAgencia)
        {
            if (idAgencia <= 0) throw new DatosInvalidosException("El id de la Agencia no puede ser menor o igual a cero");

            AgenciaDTO agenciaDTO = MapeadorAgencia.MapearAgenciaDTO(RepositorioAgencia.FindById(idAgencia)) ?? throw new DatosInvalidosException($"No se encontró una agencia con el ID {idAgencia}.");

            if (agenciaDTO == null)
            {
                throw new DatosInvalidosException($"No se encontró una agencia con el ID {idAgencia}.");
            }

            return agenciaDTO;
        }
    }
}
