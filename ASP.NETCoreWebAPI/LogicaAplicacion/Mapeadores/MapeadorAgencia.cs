using CasosUso.DTOs;
using ExcepcionesPropias;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaAplicacion.Mapeadores
{
    public class MapeadorAgencia
    {
        public static Agencia MapearAgencia(AgenciaDTO agenciaDTO)
        {
            if (agenciaDTO == null)
            {
                throw new DatosInvalidosException("El objeto AgenciaDTO no puede ser nulo.");
            }

            return new Agencia
            {
                Id = agenciaDTO.Id,
                Nombre = new NombreAgencia(agenciaDTO.Nombre),
                DireccionPostal = new DireccionPostalAgencia(agenciaDTO.DireccionPostal),
                Latitud = new LatitudAgencia(agenciaDTO.Latitud),
                Longitud = new LongitudAgencia(agenciaDTO.Longitud)
            };
        }

        public static AgenciaDTO MapearAgenciaDTO(Agencia agencia)
        {
            if (agencia == null)
            {
                throw new DatosInvalidosException("El objeto Agencia no puede ser nulo.");
            }

            return new AgenciaDTO
            {
                Id = agencia.Id,
                Nombre = agencia.Nombre.Nombre,
                DireccionPostal = agencia.DireccionPostal.DireccionPostal,
                Latitud = agencia.Latitud.Latitud,
                Longitud = agencia.Longitud.Longitud
            };
        }

        public static IEnumerable<AgenciaDTO> MapearAgenciasDTO(IEnumerable<Agencia> agencias)
        {
            if (agencias == null)
            {
                throw new DatosInvalidosException("La colección de agencias no puede ser nula.");
            }

            return agencias.Select(MapearAgenciaDTO).ToList();
        }
    }
}
