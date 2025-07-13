using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using EmpresaEnviosWebAPI.DTOs;
using Enum;
using ExcepcionesPropias;
using LogicaNegocio.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmpresaEnviosWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvioController : ControllerBase
    {
        public IAgregarEnvio AgregarEnvio { get; set; }
        public IActualizarEstado ActualizarEnvio { get; set; }
        public IBuscarEnvioPorId BuscarEnvioPorId { get; set; }
        public IEliminarEnvio EliminarEnvio { get; set; }
        public IListaEnvios ListaEnvios { get; set; }
        public IListaAgencias ListaAgencias { get; set; }
        public IBuscarUsuarioPorEmail BuscarUsuarioPorEmail { get; set; }
        public IListaEnviosComunes ListaEnviosComunes { get; set; }
        public IListaEnviosUrgentes IListaEnviosUrgentes { get; set; }
        public IBuscarEnviosPorTipoYEstado BuscarEnviosPorTipoYEstado { get; set; }
        public IFinalizarEnvio FinalizarEnvio { get; set; }
        public IBuscarEnvioPorNumeroTracking BuscarEnvioPorNumeroTracking { get; set; }
        public IBuscarSeguimientosPorEnvio BuscarSeguimientosPorEnvio { get; set; }
        public IBuscarEnviosPorCliente BuscarEnviosPorCliente { get; set; }
        public IBuscarSeguimientosPorCliente BuscarSeguimientosPorCliente { get; set; }
        public IBuscarEnviosPorEstadoYRangoFechas BuscarEnviosPorEstadoYRangoFechas { get; set; }
        public IBuscarEnviosPorComentario BuscarEnviosPorComentario { get; set; }

        public EnvioController(IAgregarEnvio agregarEnvio, IActualizarEstado actualizarEnvio, IBuscarEnvioPorId buscarEnvioPorId, IEliminarEnvio eliminarEnvio, IListaEnvios listaEnvio, IListaAgencias listaAgencia, IBuscarUsuarioPorEmail buscarUsuarioPorEmail, IListaEnviosComunes listaEnviosComunes, IListaEnviosUrgentes iListaEnviosUrgentes, IBuscarEnviosPorTipoYEstado buscarEnviosPorTipoYEstado, IFinalizarEnvio finalizarEnvio, IBuscarEnvioPorNumeroTracking buscarEnvioPorNumeroTracking, IBuscarSeguimientosPorEnvio buscarSeguimientosPorEnvio, IBuscarEnviosPorCliente buscarEnviosPorCliente, IBuscarSeguimientosPorCliente buscarSeguimientosPorCliente, IBuscarEnviosPorEstadoYRangoFechas buscarEnviosPorEstadoYRangoFechas, IBuscarEnviosPorComentario buscarEnviosPorComentario)
        {
            AgregarEnvio = agregarEnvio;
            ActualizarEnvio = actualizarEnvio;
            BuscarEnvioPorId = buscarEnvioPorId;
            EliminarEnvio = eliminarEnvio;
            ListaEnvios = listaEnvio;
            ListaAgencias = listaAgencia;
            BuscarUsuarioPorEmail = buscarUsuarioPorEmail;
            ListaEnviosComunes = listaEnviosComunes;
            IListaEnviosUrgentes = iListaEnviosUrgentes;
            BuscarEnviosPorTipoYEstado = buscarEnviosPorTipoYEstado;
            FinalizarEnvio = finalizarEnvio;
            BuscarEnvioPorNumeroTracking = buscarEnvioPorNumeroTracking;
            BuscarSeguimientosPorEnvio = buscarSeguimientosPorEnvio;
            BuscarEnviosPorCliente = buscarEnviosPorCliente;
            BuscarSeguimientosPorCliente = buscarSeguimientosPorCliente;
            BuscarEnviosPorEstadoYRangoFechas = buscarEnviosPorEstadoYRangoFechas;
            BuscarEnviosPorComentario = buscarEnviosPorComentario;
        }

        /// <summary>
        /// Obtiene un listado de envíos comunes y urgentes registrados en el sistema.
        /// </summary>
        /// <returns>
        /// 200 si se obtienen los envíos correctamente, junto con un mensaje de éxito y los datos de los envíos.
        /// 400 si hay datos inválidos en la solicitud.
        /// 404 si no se encuentran envíos registrados.
        /// 500 si ocurre un error interno del servidor.
        /// </returns>
        // GET: api/<Envio>/Listado
        [HttpGet("Listado")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Listado()
        {
            try
            {
                EnviosDTO enviosDTO = new EnviosDTO
                {
                    EnviosComunes = ListaEnviosComunes.ObtenerEnviosComunes(),
                    EnviosUrgentes = IListaEnviosUrgentes.ObtenerEnviosUrgentes()
                };

                if (enviosDTO == null ||
                    (enviosDTO.EnviosComunes == null || !enviosDTO.EnviosComunes.Any()) &&
                    (enviosDTO.EnviosUrgentes == null || !enviosDTO.EnviosUrgentes.Any()))
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontraron envíos registrados.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = "No se encontraron envíos comunes ni urgentes en el sistema.",
                        SolucionSugerida = "Registre al menos un envío antes de intentar recuperar los datos."
                    });
                }

                return Ok(new
                {
                    Mensaje = "Operación exitosa. Datos de envíos recuperados correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.Now,
                    Datos = enviosDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.Now,
                    Detalles = ex.Message,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    SolucionSugerida = "Revise la excepción interna para más detalles y valide los datos antes de reenviar la solicitud."
                });
            }

        }

        /// <summary>
        /// Obtiene los detalles de un envío común específico por su ID.
        /// </summary>
        /// <remarks>
        /// Utiliza el nombre de ruta "DetallesEnvioComun" para referenciar esta acción en otras partes de la aplicación.
        /// </remarks>
        /// <param name="id">
        /// Identificador numérico único del envío común que se desea consultar.
        /// </param>
        /// <returns>
        /// 200 si se encuentran los detalles del envío común correctamente, junto con un mensaje de éxito y los datos del envío.
        /// 400 si los datos proporcionados son inválidos.
        /// 404 si no se encuentra un envío común con el ID proporcionado.
        /// 500 si ocurre un error interno del servidor.
        /// </returns>
        // GET api/<Envio>/DetallesEnvioComun/5
        [HttpGet("DetallesEnvioComun/{id}", Name = "DetallesEnvioComun")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DetallesEnvioComun(int id)
        {
            try
            {
                EnvioDTO envioDTO = BuscarEnvioPorId.Buscar(id, TipoEnvio.Comun);

                if (envioDTO is null)
                {
                    return NotFound(new
                    {
                        Mensaje = "El envío con el ID especificado no fue encontrado.",
                        Codigo = 404,
                        IDConsultado = id, // Para mayor claridad
                        Sugerencia = "Verifique que el ID sea correcto o intente con otro ID válido.",
                        FechaConsulta = DateTime.UtcNow
                    });
                }

                envioDTO.Seguimientos = BuscarSeguimientosPorEnvio.Buscar(envioDTO.Id);

                return Ok(new
                {
                    Mensaje = "Operación exitosa. Datos de envíos recuperados correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.Now,
                    Datos = envioDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.Now,
                    Detalles = ex.Message,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    SolucionSugerida = "Revise la excepción interna para más detalles y valide los datos antes de reenviar la solicitud."
                });
            }

        }

        /// <summary>
        /// Obtiene los detalles de un envío urgente específico por su ID.
        /// </summary>
        /// <remarks>
        /// Utiliza el nombre de ruta "DetallesEnvioUrgente" para referenciar esta acción en otras partes de la aplicación.
        /// </remarks>
        /// <param name="id">
        /// Identificador numérico único del envío urgente que se desea consultar.
        /// </param>
        /// <returns>
        /// 200 si se encuentran los detalles del envío urgente correctamente, junto con un mensaje de éxito y los datos del envío.
        /// 400 si los datos proporcionados son inválidos.
        /// 404 si no se encuentra un envío urgente con el ID proporcionado.
        /// 500 si ocurre un error interno del servidor.
        /// </returns>
        // GET api/<Envio>/DetallesEnvioUrgente/5
        [HttpGet("DetallesEnvioUrgente/{id}", Name = "DetallesEnvioUrgente")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DetallesEnvioUrgente(int id)
        {
            try
            {
                EnvioDTO envioDTO = BuscarEnvioPorId.Buscar(id, TipoEnvio.Urgente);

                if (envioDTO is null)
                {
                    return NotFound(new
                    {
                        Mensaje = "El envío con el ID especificado no fue encontrado.",
                        Codigo = 404,
                        IDConsultado = id, // Para mayor claridad
                        Sugerencia = "Verifique que el ID sea correcto o intente con otro ID válido.",
                        FechaConsulta = DateTime.UtcNow
                    });
                }

                envioDTO.Seguimientos = BuscarSeguimientosPorEnvio.Buscar(envioDTO.Id);

                return Ok(new
                {
                    Mensaje = "Operación exitosa. Datos de envíos recuperados correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.Now,
                    Datos = envioDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.Now,
                    Detalles = ex.Message,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {

                    Mensaje = "Error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    SolucionSugerida = "Revise la excepción interna para más detalles y valide los datos antes de reenviar la solicitud."
                });
            }

        }

        /// <summary>
        /// Crea un nuevo envío común en el sistema.
        /// </summary>
        /// <param name="crearEnvioComunDTO">
        /// Objeto que contiene los datos necesarios para crear un envío común, incluyendo el ID de la agencia, el peso del paquete, el email del cliente y el ID del usuario que realiza la operación.
        /// </param>
        /// <returns>
        /// 201 si el envío común se crea correctamente, junto con un mensaje de éxito y los datos del envío.
        /// 400 si los datos proporcionados son inválidos o faltan.
        /// 400 si no se encuentra un cliente registrado con el email proporcionado o si el usuario no tiene el rol de 'Cliente'.
        /// 400 si la agencia seleccionada no es válida.
        /// 400 si el envío no se puede crear debido a datos inválidos.
        /// 500 si ocurre un error interno del servidor al intentar crear el envío.
        /// </returns>
        // POST api/<EnvioController>/CrearEnvioComun
        [HttpPost("CrearEnvioComun")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearEnvioComun([FromBody] CrearEnvioComunDTO crearEnvioComunDTO)
        {
            try
            {
                // Validar que la solicitud no esté vacía
                if (crearEnvioComunDTO == null)
                {
                    return BadRequest(new
                    {
                        Mensaje = "Solicitud inválida. No se recibió un envío válido.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no válidos",
                        Detalles = "Verifique que los datos enviados sean correctos y cumplan con el formato requerido.",
                        SolucionSugerida = "Asegúrese de enviar un objeto válido con los valores necesarios antes de reenviar la solicitud."
                    });
                }

                // Validar que la agencia seleccionada sea válida
                if (crearEnvioComunDTO.AgenciaId <= 0)
                {
                    return BadRequest(new
                    {
                        Mensaje = "Debe seleccionar una agencia válida para el envío.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no válidos",
                        Detalles = "El ID de la agencia no puede ser menor o igual a cero.",
                        SolucionSugerida = "Seleccione una agencia válida antes de enviar la solicitud."
                    });
                }

                // Buscar usuario en la base de datos antes de crear el envío
                UsuarioDTO usuarioDTO = BuscarUsuarioPorEmail.Buscar(crearEnvioComunDTO.EmailCliente)
                    ?? throw new DatosInvalidosException("No se encontró un cliente registrado con ese email.");

                if (usuarioDTO is null)
                {
                    return BadRequest(new
                    {
                        Mensaje = "No se encontró un cliente registrado con ese email en el sistema.",
                        Codigo = 400,
                        FechaError = DateTime.Now,
                        TipoError = "Datos no válidos",
                        Detalles = "El email proporcionado no corresponde a ningún cliente registrado.",
                        SolucionSugerida = "Verifique la dirección de correo electrónico o registre al cliente antes de agendar el envío."
                    });
                }

                if (usuarioDTO.NombreRol != "Cliente")
                {
                    return BadRequest(new
                    {
                        Mensaje = "El usuario existe en la base de datos pero no tiene el rol asignado de 'Cliente'.",
                        Codigo = 400,
                        FechaError = DateTime.Now,
                        TipoError = "Datos no válidos",
                        Detalles = "Se encontró el usuario en la base de datos con un rol distinto: '" + usuarioDTO.NombreRol + "'.",
                        SolucionSugerida = "Verificar y actualizar el rol del usuario a 'Cliente' si corresponde."
                    });
                }

                // Crear instancia y convertir el peso
                EnvioDTO envioDTO = new EnvioDTO();

                // Asignar el resto de las propiedades
                envioDTO.NumeroTracking = NumeroTrackingEnvio.GenerarNumeroTracking();
                envioDTO.EmpleadoId = crearEnvioComunDTO.UsuarioId;
                envioDTO.ClienteId = usuarioDTO.Id;
                envioDTO.Peso = envioDTO.ConvertirPesoAGramos(crearEnvioComunDTO.Peso);
                envioDTO.FechaSalida = DateTime.UtcNow;
                envioDTO.Estado = EstadoEnvio.EnProcesoEntrega;
                envioDTO.AgenciaId = crearEnvioComunDTO.AgenciaId;

                // Guardar el envío en la base de datos
                AgregarEnvio.Agregar(envioDTO, crearEnvioComunDTO.UsuarioId);

                return CreatedAtRoute("DetallesEnvioComun", new { id = envioDTO.Id }, new
                {
                    Mensaje = "Operación exitosa. Envío común creado correctamente.",
                    Codigo = 201,
                    Fecha = DateTime.UtcNow,
                    Datos = envioDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise la excepción interna para más detalles y valide los datos antes de reenviar la solicitud."
                });
            }
        }

        /// <summary>
        /// Crea un nuevo envío urgente en el sistema.
        /// </summary>
        /// <param name="crearEnvioUrgenteDTO">
        /// Objeto que contiene los datos necesarios para crear un envío urgente, incluyendo el peso del paquete, el email del cliente y el ID del usuario que realiza la operación.
        /// </param>
        /// <returns>
        /// 201 si el envío urgente se crea correctamente, junto con un mensaje de éxito y los datos del envío.
        /// 400 si los datos proporcionados son inválidos o faltan.
        /// 400 si no se encuentra un cliente registrado con el email proporcionado o si el usuario no tiene el rol de 'Cliente'.
        /// 400 si el envío no se puede crear debido a datos inválidos.
        /// 500 si ocurre un error interno del servidor al intentar crear el envío urgente.
        /// </returns>
        [HttpPost("CrearEnvioUrgente")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearEnvioUrgente([FromBody] CrearEnvioUrgenteDTO crearEnvioUrgenteDTO)
        {
            try
            {
                if (crearEnvioUrgenteDTO == null)
                {
                    return BadRequest(new
                    {
                        Mensaje = "Solicitud inválida. No se recibió un envío válido.",
                        Codigo = 400,
                        FechaError = DateTime.Now,
                        TipoError = "Datos no válidos",
                        Detalles = "Verifique que los datos enviados sean correctos y cumplan con el formato requerido.",
                        SolucionSugerida = "Asegúrese de enviar un objeto válido con los valores necesarios antes de reenviar la solicitud."
                    });
                }

                UsuarioDTO usuarioDTO = BuscarUsuarioPorEmail.Buscar(crearEnvioUrgenteDTO.EmailCliente);

                if (usuarioDTO is null)
                {
                    return BadRequest(new
                    {
                        Mensaje = "No se encontró un cliente registrado con ese email en el sistema.",
                        Codigo = 400,
                        FechaError = DateTime.Now,
                        TipoError = "Datos no válidos",
                        Detalles = "El email proporcionado no corresponde a ningún cliente registrado.",
                        SolucionSugerida = "Verifique la dirección de correo electrónico o registre al cliente antes de agendar el envío."
                    });
                }

                if (usuarioDTO.NombreRol != "Cliente")
                {
                    return BadRequest(new
                    {
                        Mensaje = "El usuario existe en la base de datos pero no tiene el rol asignado de 'Cliente'.",
                        Codigo = 400,
                        FechaError = DateTime.Now,
                        TipoError = "Datos no válidos",
                        Detalles = "Se encontró el usuario en la base de datos con un rol distinto: '" + usuarioDTO.NombreRol + "'.",
                        SolucionSugerida = "Verificar y actualizar el rol del usuario a 'Cliente' si corresponde."
                    });
                }

                EnvioDTO envioDTO = new EnvioDTO();

                envioDTO.NumeroTracking = NumeroTrackingEnvio.GenerarNumeroTracking();
                envioDTO.EmpleadoId = crearEnvioUrgenteDTO.UsuarioId;
                envioDTO.ClienteId = usuarioDTO.Id;
                envioDTO.Peso = envioDTO.ConvertirPesoAGramos(crearEnvioUrgenteDTO.Peso);
                envioDTO.FechaSalida = DateTime.UtcNow;
                envioDTO.Estado = EstadoEnvio.EnProcesoEntrega;
                envioDTO.DireccionPostal = crearEnvioUrgenteDTO.DireccionPostal;

                AgregarEnvio.Agregar(envioDTO, crearEnvioUrgenteDTO.UsuarioId);

                return CreatedAtRoute("DetallesEnvioUrgente", new { id = envioDTO.Id }, new
                {
                    Mensaje = "Operación exitosa. Envío creado correctamente.",
                    Codigo = 201,
                    Fecha = DateTime.Now,
                    Datos = envioDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.Now,
                    TipoError = "Datos no válidos",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise la excepción interna para más detalles y valide los datos antes de reenviar la solicitud."
                });
            }
        }

        /// <summary>
        /// Obtiene un listado de envíos que están en proceso de entrega, tanto comunes como urgentes.
        /// </summary>
        /// <returns>
        /// 200 si se obtienen los envíos en proceso de entrega correctamente, junto con un mensaje de éxito y los datos de los envíos.
        /// 400 si hay datos inválidos en la solicitud.
        /// 404 si no se encuentran envíos en proceso de entrega.
        /// 500 si ocurre un error interno del servidor al intentar recuperar los envíos.
        /// </returns>
        // GET api/envio/ListadoProcesoEntrega
        [HttpGet("ListadoProcesoEntrega")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ListadoProcesoEntrega()
        {
            try
            {
                // Obtener los envíos en proceso de entrega
                var enviosComunes = BuscarEnviosPorTipoYEstado.Buscar(TipoEnvio.Comun, EstadoEnvio.EnProcesoEntrega);
                var enviosUrgentes = BuscarEnviosPorTipoYEstado.Buscar(TipoEnvio.Urgente, EstadoEnvio.EnProcesoEntrega);

                if ((enviosComunes == null || !enviosComunes.Any()) && (enviosUrgentes == null || !enviosUrgentes.Any()))
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontraron envíos en proceso de entrega.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = "No hay envíos registrados en proceso de entrega en el sistema.",
                        SolucionSugerida = "Verifique que haya envíos registrados en esta etapa del proceso."
                    });
                }

                EnviosDTO enviosDTO = new EnviosDTO
                {
                    EnviosComunes = enviosComunes.ToList(),
                    EnviosUrgentes = enviosUrgentes.ToList()
                };

                return Ok(new
                {
                    Mensaje = "Lista de envíos en proceso de entrega obtenida exitosamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = enviosDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    SolucionSugerida = "Revise los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Se produjo un error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise la excepción interna para más detalles o contacte a soporte técnico."
                });
            }
        }

        /// <summary>
        /// Finaliza un envío específico por su ID y tipo de envío.
        /// </summary>
        /// <param name="id">
        /// Identificador numérico único del envío que se desea finalizar.
        /// </param>
        /// <param name="finalizarDTO">
        /// Objeto que contiene los datos necesarios para finalizar el envío, incluyendo el ID del usuario que realiza la operación y el tipo de envío.
        /// </param>
        /// <returns>
        /// 200 si el envío se finaliza correctamente, junto con un mensaje de éxito y los datos del envío.
        /// 400 si los datos proporcionados son inválidos o faltan.
        /// 401 si el usuario no está autenticado o no tiene el rol adecuado.
        /// 404 si no se encuentra un envío con el ID y tipo proporcionados.
        /// 500 si ocurre un error interno del servidor al intentar finalizar el envío.
        /// </returns>
        // POST api/envio/Finalizar/{id}
        [HttpPost("Finalizar/{id}")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Finalizar(int id, [FromBody] FinalizarDTO finalizarDTO)
        {
            try
            {
                // Validar que el DTO no sea nulo
                if (finalizarDTO == null)
                {
                    return BadRequest(new
                    {
                        Mensaje = "Los datos enviados son nulos o incorrectos.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no válidos",
                        Detalles = "El cuerpo de la solicitud no puede estar vacío o mal formado.",
                        SolucionSugerida = "Asegúrese de enviar un objeto válido en el cuerpo de la solicitud."
                    });
                }

                // Buscar el envío por ID y tipo
                EnvioDTO envioDTO = BuscarEnvioPorId.Buscar(id, finalizarDTO.TipoEnvio);

                if (envioDTO is null)
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontró un envío con el identificador y tipo proporcionado.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = $"No se encontró un envío con ID {id} y tipo {finalizarDTO.TipoEnvio}.",
                        SolucionSugerida = "Verifique los datos ingresados y vuelva a intentarlo."
                    });
                }

                // Validar que el usuario esté autenticado
                if (finalizarDTO.UsuarioId <= 0)
                {
                    return Unauthorized(new
                    {
                        Mensaje = "No se encontró un usuario válido. Inicie sesión para continuar.",
                        Codigo = 401,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario no autenticado",
                        Detalles = "El ID de usuario proporcionado es inválido o no está autenticado.",
                        SolucionSugerida = "Asegúrese de estar autenticado antes de realizar esta acción."
                    });
                }

                // Finalizar el envío
                FinalizarEnvio.Finalizar(envioDTO, finalizarDTO.UsuarioId);

                return Ok(new
                {
                    Mensaje = $"El envío con ID {envioDTO.Id} ha sido finalizado correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = envioDTO,
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Error en los datos proporcionados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise los datos enviados y asegúrese de que estén correctamente formateados."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Se produjo un error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise la excepción interna o contacte al soporte técnico."
                });
            }
        }

        /// <summary>
        /// Actualiza el estado de un envío específico por su ID y tipo de envío.
        /// </summary>
        /// <param name="id">
        /// Identificador numérico único del envío que se desea actualizar.
        /// </param>
        /// <param name="tipoEnvio">
        /// Tipo de envío que se está actualizando (Común o Urgente).
        /// </param>
        /// <param name="actualizarEstadoDTO">
        /// Objeto que contiene los datos necesarios para actualizar el estado del envío, incluyendo el ID del usuario que realiza la operación, el estado seleccionado y un comentario opcional.
        /// </param>
        /// <returns>
        /// 200 si el estado del envío se actualiza correctamente, junto con un mensaje de éxito y los datos del envío actualizado.
        /// 401 si el usuario no está autenticado o no tiene el rol adecuado.
        /// 400 si los datos proporcionados son inválidos o faltan, incluyendo estados no válidos o selección de 'Todos'.
        /// 400 si no se encuentra un envío con el ID y tipo proporcionados.
        /// 400 si el estado seleccionado no es válido o es 'Todos'.
        /// 400 datos inválidos en la solicitud, como un estado no válido o un usuario no autenticado.
        /// 404 si no se encuentra un envío con el ID y tipo proporcionados.
        /// 500 si ocurre un error interno del servidor al intentar actualizar el estado del envío.
        /// </returns>
        // PUT api/envio/ActualizarEstado/{id}/{tipoEnvio}
        [HttpPost("ActualizarEstado/{id}/{tipoEnvio}")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarEstado(int id, TipoEnvio tipoEnvio, [FromBody] ActualizarEstadoDTO actualizarEstadoDTO)
        {
            try
            {
                if (actualizarEstadoDTO == null)
                {
                    return BadRequest(new
                    {
                        Mensaje = "Los datos enviados son nulos o incorrectos.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no válidos",
                        Detalles = "El cuerpo de la solicitud no puede estar vacío o mal formado.",
                        SolucionSugerida = "Asegúrese de enviar un objeto válido en el cuerpo de la solicitud."
                    });
                }

                if (!System.Enum.IsDefined(typeof(EstadoEnvio), actualizarEstadoDTO.EstadoSeleccionado))
                {
                    return BadRequest(new
                    {
                        Mensaje = $"El estado seleccionado ('{actualizarEstadoDTO.EstadoSeleccionado}') no es válido.",
                        Codigo = 400,
                        FechaError = DateTime.Now,
                        TipoError = "Datos no válidos",
                        Detalles = "El valor recibido no coincide con ninguno de los estados definidos en el sistema.",
                        SolucionSugerida = "Verificar que el estado seleccionado sea uno de los valores válidos del enumerador EstadoEnvio."
                    });
                }

                if (actualizarEstadoDTO.EstadoSeleccionado == (int)EstadoEnvio.Todos)
                {
                    return BadRequest(new
                    {
                        Mensaje = "No se puede actualizar el estado porque se seleccionó 'Todos', lo cual no representa un estado individual válido.",
                        Codigo = 400,
                        FechaError = DateTime.Now,
                        TipoError = "Selección no válida",
                        Detalles = "El valor 'Todos' es solo para filtrado o visualización, no se puede usar para actualizar el estado de un envío.",
                        SolucionSugerida = "Seleccione un estado específico como 'En Proceso Entrega' o 'Finalizado'."
                    });
                }

                // Buscar el envío por ID y tipo
                EnvioDTO envioDTO = BuscarEnvioPorId.Buscar(id, tipoEnvio);

                if (envioDTO is null)
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontró un envío con el identificador y tipo proporcionado.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = $"No se encontró un envío con ID {id} y tipo {tipoEnvio}.",
                        SolucionSugerida = "Verifique los datos ingresados y vuelva a intentarlo."
                    });
                }

                // Validar que el usuario esté autenticado
                if (actualizarEstadoDTO.UsuarioId <= 0)
                {
                    return Unauthorized(new
                    {
                        Mensaje = "No se encontró un usuario válido. Inicie sesión para continuar.",
                        Codigo = 401,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario no autenticado",
                        Detalles = "El ID de usuario proporcionado es inválido o no está autenticado.",
                        SolucionSugerida = "Asegúrese de estar autenticado antes de realizar esta acción."
                    });
                }

                // Actualizar el estado del envío
                envioDTO.Estado = actualizarEstadoDTO.EstadoSeleccionado;

                envioDTO.Comentario = actualizarEstadoDTO.Comentario ?? "Sin comentarios adicionales.";

                ActualizarEnvio.Actualizar(envioDTO, actualizarEstadoDTO.UsuarioId, actualizarEstadoDTO.Comentario);

                return Ok(new
                {
                    Mensaje = $"El estado del envío con ID {envioDTO.Id} ha sido actualizado exitosamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = envioDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Error en los datos proporcionados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise los datos enviados y asegúrese de que estén correctamente formateados."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Se produjo un error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise la excepción interna para más detalles o contacte al soporte técnico."
                });
            }
        }

        /// <summary>
        /// Busca un envío por su número de tracking y devuelve sus detalles junto con los seguimientos asociados.
        /// </summary>
        /// <param name="buscarTrackingDTO">
        /// Objeto que contiene el número de tracking del envío que se desea buscar.
        /// </param>
        /// <returns>
        /// 200 si se encuentra el envío con el número de tracking proporcionado, junto con un mensaje de éxito y los datos del envío.
        /// 400 si el número de tracking es nulo o vacío.
        /// 400 si los datos proporcionados son inválidos o faltan.
        /// 404 si no se encuentra un envío con el número de tracking proporcionado.
        /// 500 si ocurre un error interno del servidor al intentar buscar el envío.
        /// </returns>
        // POST api/envio/BuscarTracking
        [HttpPost("BuscarTracking")]
        //[Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BuscarTracking([FromBody] BuscarTrackingDTO buscarTrackingDTO)
        {
            try
            {
                if (buscarTrackingDTO == null || string.IsNullOrEmpty(buscarTrackingDTO.NumeroTracking))
                {
                    return BadRequest(new
                    {
                        Mensaje = "El número de tracking es obligatorio.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no válidos",
                        Detalles = "El cuerpo de la solicitud debe contener un número de tracking válido.",
                        SolucionSugerida = "Asegúrese de enviar un número de tracking válido en el cuerpo de la solicitud."
                    });
                }

                // Simulación de búsqueda en la base de datos
                EnvioDTO envioDTO = BuscarEnvioPorNumeroTracking.Buscar(buscarTrackingDTO.NumeroTracking);

                if (envioDTO == null)
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontró un envío con el número de tracking proporcionado.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = $"No se encontró un envío con el número de tracking '{buscarTrackingDTO.NumeroTracking}'.",
                        SolucionSugerida = "Verifique que el número de tracking sea correcto e intente nuevamente."
                    });
                }

                envioDTO.Seguimientos = BuscarSeguimientosPorEnvio.Buscar(envioDTO.Id);

                return Ok(new
                {
                    Mensaje = "El envío ha sido localizado con éxito.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = envioDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise los datos enviados y asegúrese de que estén correctamente formateados."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise la excepción interna para más detalles y valide los datos antes de reenviar la solicitud."
                });
            }

        }

        /// <summary>
        /// Obtiene un envío específico por su ID y tipo de envío.
        /// </summary>
        /// <param name="id">
        /// Identificador numérico único del envío que se desea obtener.
        /// </param>
        /// <param name="tipoEnvio">
        /// Tipo de envío que se está buscando (Común o Urgente).
        /// </param>
        /// <returns>
        /// 200 si se encuentra el envío con el ID y tipo proporcionados, junto con un mensaje de éxito y los datos del envío.
        /// 400 si los datos proporcionados son inválidos o faltan.
        /// 404 si no se encuentra un envío con el ID y tipo proporcionados.
        /// 500 si ocurre un error interno del servidor al intentar obtener el envío.
        /// </returns>
        // GET api/envio/ObtenerPorId/{id}/{tipoEnvio}
        [HttpGet("BuscarPorIdYTipo/{id}/{tipoEnvio}")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ObtenerPorId(int id, TipoEnvio tipoEnvio)
        {
            try
            {
                // Buscar el envío en la base de datos
                EnvioDTO envioDTO = BuscarEnvioPorId.Buscar(id, tipoEnvio);

                if (envioDTO is null)
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontró un envío con el identificador y tipo proporcionado.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = $"No se encontró un envío con ID {id} y tipo {tipoEnvio}.",
                        SolucionSugerida = "Verifique que los datos ingresados sean correctos."
                    });
                }

                return Ok(new
                {
                    Mensaje = "Envío obtenido correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = envioDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Error en los datos proporcionados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.Message ?? ex.Message,
                    SolucionSugerida = "Verifique que los datos ingresados sean correctos antes de reintentar."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Se produjo un error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise la excepción interna para más detalles."
                });
            }
        }

        /// <summary>
        /// Obtiene un listado de envíos realizados por un cliente específico, filtrando por su ID.
        /// </summary>
        /// <param name="clienteId">
        /// ID del cliente cuyos envíos se desean listar. Debe ser un número positivo.
        /// </param>
        /// <returns>
        /// 200 si se obtienen los envíos del cliente correctamente, junto con un mensaje de éxito y los datos de los envíos.
        /// 400 si el ID del cliente es inválido (menor o igual a cero).
        /// 400 si los datos proporcionados son inválidos o faltan.
        /// 404 si no se encuentran envíos para el cliente especificado.
        /// 500 si ocurre un error interno del servidor al intentar recuperar los envíos.
        /// </returns>
        // GET: api/envio/ListadoCliente
        [HttpGet("ListadoCliente/{clienteId}")]
        [Authorize(Roles = "Cliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ListadoCliente(int clienteId)
        {
            try
            {
                if (clienteId <= 0)
                {
                    return BadRequest(new
                    {
                        Mensaje = "El ID del cliente debe ser un número positivo.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no válidos",
                        Detalles = "El ID del cliente debe ser mayor que cero.",
                        SolucionSugerida = "Proporcione un ID de cliente válido."
                    });
                }

                // Buscar envíos por cliente
                IEnumerable<EnvioDTO> enviosDTO = BuscarEnviosPorCliente.Buscar(clienteId);

                if (enviosDTO == null || !enviosDTO.Any())
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontraron envíos para el cliente especificado.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = $"No se encontraron envíos para el cliente con ID {clienteId}.",
                        SolucionSugerida = "Verifique que el ID del cliente sea correcto o que existan envíos registrados."
                    });
                }

                EnviosDTO envios = new EnviosDTO
                {
                    EnviosComunes = enviosDTO.Where(e => e.Tipo == TipoEnvio.Comun).ToList(),
                    EnviosUrgentes = enviosDTO.Where(e => e.Tipo == TipoEnvio.Urgente).ToList()
                };

                return Ok(new
                {
                    Mensaje = "Lista de envíos del cliente obtenida exitosamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = envios
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise la excepción interna para más detalles y valide los datos antes de reenviar la solicitud."
                });
            }
        }

        /// <summary>
        /// Busca envíos por estado y rango de fechas, filtrando los resultados por el ID del cliente.
        /// </summary>
        /// <param name="estadoEnvio">
        /// Estado del envío que se desea buscar (EnProcesoEntrega, Finalizado, etc.).
        /// </param>
        /// <param name="fechaInicio">
        /// Fecha de inicio del rango de fechas para filtrar los envíos.
        /// </param>
        /// <param name="fechaFin">
        /// Fecha de fin del rango de fechas para filtrar los envíos.
        /// </param>
        /// <param name="idCliente">
        /// ID del cliente cuyos envíos se desean listar. Debe ser un número positivo.
        /// </param>
        /// <returns>
        /// 200 si se encuentran envíos para el estado y rango de fechas especificados, junto con un mensaje de éxito y los datos de los envíos.
        /// 400 si el rango de fechas es inválido (fecha de inicio mayor que fecha de fin) o si el ID del cliente es inválido (menor o igual a cero).
        /// 400 si los datos proporcionados son inválidos o faltan.
        /// 404 si no se encuentran envíos para el estado y rango de fechas especificados.
        /// 500 si ocurre un error interno del servidor al intentar recuperar los envíos.
        /// </returns>
        // GET: api/Envio/BuscarPorEstadoYRangoFechas
        [HttpGet("BuscarPorEstadoYRangoFechas/{estadoEnvio}/{fechaInicio}/{fechaFin}")]
        [Authorize(Roles = "Cliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BuscarPorEstadoYRangoFechas([FromRoute] EstadoEnvio estadoEnvio, DateTime fechaInicio, DateTime fechaFin, [FromQuery] int idCliente)
        {
            try
            {
                // Validar que el rango de fechas sea correcto
                if (fechaInicio > fechaFin)
                {
                    return BadRequest(new
                    {
                        Mensaje = "La fecha de inicio no puede ser mayor que la fecha de fin.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no válidos",
                        Detalles = "El rango de fechas proporcionado es incorrecto.",
                        SolucionSugerida = "Asegúrese de que la fecha de inicio sea anterior a la fecha de fin."
                    });
                }

                // Buscar envíos por estado y rango de fechas
                IEnumerable<EnvioDTO> enviosDTO = BuscarEnviosPorEstadoYRangoFechas.Buscar(estadoEnvio, fechaInicio, fechaFin, idCliente);

                if (enviosDTO == null || !enviosDTO.Any())
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontraron envíos para el estado y rango de fechas especificados.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = $"No se encontraron envíos con estado '{estadoEnvio}' entre {fechaInicio} y {fechaFin}.",
                        SolucionSugerida = "Verifique los parámetros ingresados y vuelva a intentarlo."
                    });
                }

                EnviosDTO envios = new EnviosDTO
                {
                    EnviosComunes = enviosDTO.Where(e => e.Tipo == TipoEnvio.Comun).ToList(),
                    EnviosUrgentes = enviosDTO.Where(e => e.Tipo == TipoEnvio.Urgente).ToList()
                };

                return Ok(new
                {
                    Mensaje = "Envíos encontrados exitosamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = envios
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
        }

        /// <summary>
        /// Busca envíos por un comentario específico y devuelve los envíos asociados al cliente con el ID proporcionado.
        /// </summary>
        /// <param name="Comentario">
        /// Comentario que se utilizará para filtrar los envíos.
        /// </param>
        /// <param name="idCliente">
        /// ID del cliente cuyos envíos se desean listar. Debe ser un número positivo.
        /// </param>
        /// <returns>
        /// 200 si se encuentran envíos que coinciden con el comentario proporcionado, junto con un mensaje de éxito y los datos de los envíos.
        /// 400 si el comentario es nulo o vacío, o si el ID del cliente es inválido (menor o igual a cero).
        /// 400 si los datos proporcionados son inválidos o faltan.
        /// 400 si el comentario proporcionado no es válido o está vacío.
        /// 404 si no se encuentran envíos que coincidan con el comentario ingresado para el cliente especificado.
        /// 500 si ocurre un error interno del servidor al intentar recuperar los envíos.
        /// </returns>
        // GET: api/Envio/FormBuscarPorComentario/{Comentario}
        [HttpGet("BuscarPorComentario/{Comentario}")]
        [Authorize(Roles = "Cliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BuscarPorComentario([FromRoute] string Comentario, int idCliente)
        {
            try
            {
                if (string.IsNullOrEmpty(Comentario))
                {
                    return BadRequest(new
                    {
                        Mensaje = "El comentario proporcionado no puede estar vacío.",
                        CodigoError = 400,
                        FechaError = DateTime.Now,
                        TipoError = "Validación de entrada",
                        Detalles = $"El parámetro 'Comentario' es obligatorio para realizar la búsqueda del cliente con ID {idCliente}.",
                        SolucionSugerida = "Asegúrate de enviar un comentario válido como parte de la URL."
                    });
                }

                if (idCliente <= 0)
                {
                    return BadRequest(new
                    {
                        Mensaje = "El identificador del cliente debe ser un número mayor que cero.",
                        CodigoError = 400,
                        FechaError = DateTime.Now,
                        TipoError = "Validación de entrada",
                        Detalles = "El parámetro 'idCliente' recibido no es válido o está ausente.",
                        SolucionSugerida = "Asegúrate de enviar un valor numérico válido mayor que cero para 'idCliente'."
                    });
                }

                IEnumerable<EnvioDTO> enviosDTO = BuscarEnviosPorComentario.Buscar(Comentario, idCliente);

                if (enviosDTO is null || !enviosDTO.Any())
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontraron envíos que coincidan con el comentario ingresado.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = $"No se encontraron envíos asociados al comentario '{Comentario}' para el cliente con ID {idCliente}.",
                        SolucionSugerida = "Verifique que el término ingresado sea correcto o intente con otra palabra clave."
                    });
                }

                EnviosDTO envios = new EnviosDTO
                {
                    EnviosComunes = enviosDTO.Where(e => e.Tipo == TipoEnvio.Comun).ToList(),
                    EnviosUrgentes = enviosDTO.Where(e => e.Tipo == TipoEnvio.Urgente).ToList()
                };

                return Ok(new
                {
                    Mensaje = "Envíos encontrados exitosamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = envios
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error inesperado en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
        }
    }
}