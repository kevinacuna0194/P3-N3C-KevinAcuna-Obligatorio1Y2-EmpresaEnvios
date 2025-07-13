using CasosUso.DTOs;
using Enum;
using ExcepcionesPropias;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaAplicacion.Mapeadores
{
    public class MapeadorEnvio
    {
        public static Envio MapearEnvio(EnvioDTO envioDTO)
        {
            if (envioDTO is null)
            {
                throw new DatosInvalidosException("El EnvioDTO no puede ser nulo.");
            }

            Envio envio = null;

            if (envioDTO.AgenciaId is not null)
            {
                envio = new EnvioComun
                {
                    Agencia = new Agencia { Id = envioDTO.AgenciaId.Value }
                };
            }
            else
            {
                envio = new EnvioUrgente
                {
                    DireccionPostal = new DireccionPostalEnvioUrgente(envioDTO.DireccionPostal),
                    EntregaEficiente = envioDTO.EntregaEficiente ?? false
                };
            }

            envio.Id = envioDTO.Id;
            envio.NumeroTracking = new NumeroTrackingEnvio(envioDTO.NumeroTracking);
            envio.Empleado = new Usuario { Id = envioDTO.EmpleadoId };
            envio.Cliente = new Usuario { Id = envioDTO.ClienteId };
            envio.Peso = new PesoEnvio(envioDTO.Peso);
            envio.FechaSalida = new FechaSalidaEnvio(envioDTO.FechaSalida);
            envio.FechaEntrega = envioDTO.FechaEntrega != null ? new FechaEntregaEnvio(envioDTO.FechaEntrega.Value) : null;
            envio.Estado = envioDTO.Estado;

            return envio;
        }

        public static EnvioDTO MapearEnvioDTO(Envio envio)
        {
            if (envio is null)
            {
                throw new DatosInvalidosException("El Envio no puede ser nulo.");
            }

            EnvioDTO envioDTO = null;

            if (envio is EnvioComun envioComun)
            {
                return envioDTO = new EnvioDTO
                {
                    Id = envio.Id,
                    NumeroTracking = envio.NumeroTracking.NumeroTracking,
                    EmpleadoId = envio.Empleado.Id,
                    NombreEmpleado = envio.Empleado.Nombre.Nombre,
                    ClienteId = envio.Cliente.Id,
                    NombreCliente = envio.Cliente.Nombre.Nombre,
                    Peso = envio.Peso.Peso,
                    FechaSalida = envio.FechaSalida.FechaSalida,
                    FechaEntrega = envio.FechaEntrega?.FechaEntrega,
                    Estado = envio.Estado,
                    AgenciaId = envioComun.Agencia.Id,
                    NombreAgencia = envioComun.Agencia.Nombre.Nombre,
                    EmailCliente = envio.Cliente.Email.Email,
                    Seguimientos = MapeadorSeguimiento.MapearListaSeguimientoDTO(envio.Seguimientos),
                    Tipo = TipoEnvio.Comun
                };
            }
            else if (envio is EnvioUrgente envioUrgente)
            {
                return envioDTO = new EnvioDTO
                {
                    Id = envio.Id,
                    NumeroTracking = envio.NumeroTracking.NumeroTracking,
                    EmpleadoId = envio.Empleado.Id,
                    NombreEmpleado = envio.Empleado.Nombre.Nombre,
                    ClienteId = envio.Cliente.Id,
                    NombreCliente = envio.Cliente.Nombre.Nombre,
                    Peso = envio.Peso.Peso,
                    FechaSalida = envio.FechaSalida.FechaSalida,
                    FechaEntrega = envio.FechaEntrega?.FechaEntrega,
                    Estado = envio.Estado,
                    DireccionPostal = envioUrgente.DireccionPostal.DireccionPostal,
                    EntregaEficiente = envioUrgente.EntregaEficiente,
                    EmailCliente = envio.Cliente.Email.Email,
                    Seguimientos = MapeadorSeguimiento.MapearListaSeguimientoDTO(envio.Seguimientos),
                    Tipo = TipoEnvio.Urgente
                };
            }
            else
            {
                throw new DatosInvalidosException("Tipo de envío no reconocido.");
            }
        }

        public static IEnumerable<EnvioDTO> MapearListaEnviosDTO(IEnumerable<Envio> envios)
        {
            if (envios is null)
            {
                throw new DatosInvalidosException("La lista de envíos no puede ser nula.");
            }

            return envios.Select(envio => MapearEnvioDTO(envio)).ToList();
        }
    }
}
