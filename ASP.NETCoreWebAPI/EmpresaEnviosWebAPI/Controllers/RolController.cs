using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmpresaEnviosWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        public IListaRoles ListaRoles { get; set; }
        public IBuscarNombreRolPorId BuscarNombreRolPorId { get; set; }

        public RolController(IListaRoles listaRoles, IBuscarNombreRolPorId buscarNombreRolPorId)
        {
            ListaRoles = listaRoles;
            BuscarNombreRolPorId = buscarNombreRolPorId;
        }

        /// <summary>
        /// Obtiene un listado de todos los roles registrados en el sistema.
        /// </summary>
        /// <returns>
        /// 200 si se obtienen los roles correctamente, junto con un mensaje de éxito y los datos de los roles.
        /// 400 si hay datos inválidos en la solicitud.
        /// 404 si no se encuentran roles registrados.
        /// 500 si ocurre un error interno del servidor al procesar la solicitud.
        /// </returns>
        // GET: api/<RolController>/Listado
        [HttpGet("Listado")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Listado()
        {
            try
            {
                IEnumerable<RolDTO> roles = ListaRoles.ObtenerListaRoles();

                if (!roles.Any())
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontraron roles.",
                        Codigo = 404,
                        Fecha = DateTime.UtcNow,
                        TipoError = "No Encontrado",
                        Detalles = "La lista de roles está vacía.",
                        SolucionSugerida = "Asegúrese de que existan roles en el sistema antes de realizar esta solicitud."
                    });
                }

                return Ok(new
                {
                    Mensaje = "Operación exitosa. Datos de roles recuperados correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = roles
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos Inválidos",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique los datos enviados y asegúrese de que cumplan con el formato requerido."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno del servidor al procesar la solicitud.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Excepción no controlada",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Intente nuevamente más tarde o contacte al soporte técnico."
                });
            }
        }

        /// <summary>
        /// Obtiene el nombre de un rol específico por su ID.
        /// </summary>
        /// <param name="id">
        /// Identificador numérico único del rol cuyo nombre se desea recuperar.
        /// </param>
        /// <returns>
        /// 200 si se encuentra el nombre del rol correctamente, junto con un mensaje de éxito y los datos del rol.
        /// 400 si el ID del rol es inválido o menor o igual a cero.
        /// 400 si los datos proporcionados son inválidos o no cumplen con los requisitos esperados.
        /// 404 si no se encuentra un rol con el ID proporcionado.
        /// 404 si no se encuentra un rol con el ID proporcionado.
        /// 500 si ocurre un error interno del servidor al procesar la solicitud.
        /// </returns>
        // GET api/<RolController>/5
        [HttpGet("Nombre/{id}")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Nombre(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        Mensaje = "El ID del rol debe ser mayor que cero.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos Inválidos",
                        Detalles = "El ID proporcionado no es válido.",
                        SolucionSugerida = "Proporcione un ID de rol válido."
                    });
                }

                string nombreRol = BuscarNombreRolPorId.Buscar(id);

                if (string.IsNullOrEmpty(nombreRol))
                {
                    return NotFound(new
                    {
                        Mensaje = $"No se encontró un rol con el ID: {id}",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "No Encontrado",
                        Detalles = $"No se encontró un rol con el ID: {id}. Verifique que el ID sea correcto.",
                        SolucionSugerida = "Verifique el ID del rol y vuelva a intentarlo."
                    });
                }

                RolDTO rol = new RolDTO
                {
                    Id = id,
                    Nombre = nombreRol
                };

                return Ok(new
                {
                    Mensaje = "Operación exitosa. Nombre del rol recuperado correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = rol
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = "Los datos proporcionados no cumplen con los requisitos esperados.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos Inválidos",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique el ID del rol y asegúrese de que sea válido."
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    Mensaje = ex.Message,
                    Codigo = 404,
                    FechaError = DateTime.UtcNow,
                    TipoError = "No Encontrado",
                    Detalles = $"No se encontró un rol con el ID: {id}. Verifique que el ID sea correcto.",
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique el ID del rol y vuelva a intentarlo."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno del servidor al procesar la solicitud.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Excepción no controlada",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Intente nuevamente más tarde o contacte al soporte técnico."
                });
            }
        }

        // POST api/<RolController>
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status501NotImplemented)]
        public IActionResult Post([FromBody] string value)
        {
            // Este método no está implementado en el contexto actual.
            // Podría ser utilizado para crear un nuevo rol, pero no se ha definido la lógica.
            // Se recomienda implementar la lógica necesaria o eliminar este método si no es necesario.
            return StatusCode(501, new
            {
                Mensaje = "Método no implementado.",
                Codigo = 501,
                FechaError = DateTime.UtcNow,
                TipoError = "No Implementado",
                Detalles = "Este método aún no ha sido implementado.",
                SolucionSugerida = "Considere implementar la lógica necesaria para crear un nuevo rol."
            });
        }

        // PUT api/<RolController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(statusCode: StatusCodes.Status501NotImplemented)]
        public IActionResult Put(int id, [FromBody] string value)
        {
            // Este método no está implementado en el contexto actual.
            // Podría ser utilizado para actualizar un rol existente, pero no se ha definido la lógica.
            // Se recomienda implementar la lógica necesaria o eliminar este método si no es necesario.
            return StatusCode(501, new
            {
                Mensaje = "Método no implementado.",
                Codigo = 501,
                FechaError = DateTime.UtcNow,
                TipoError = "No Implementado",
                Detalles = "Este método aún no ha sido implementado.",
                SolucionSugerida = "Considere implementar la lógica necesaria para actualizar un rol existente."
            });
        }

        // DELETE api/<RolController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(statusCode: StatusCodes.Status501NotImplemented)]
        public IActionResult Delete(int id)
        {
            // Este método no está implementado en el contexto actual.
            // Podría ser utilizado para eliminar un rol existente, pero no se ha definido la lógica.
            // Se recomienda implementar la lógica necesaria o eliminar este método si no es necesario.
            return StatusCode(501, new
            {
                Mensaje = "Método no implementado.",
                Codigo = 501,
                FechaError = DateTime.UtcNow,
                TipoError = "No Implementado",
                Detalles = "Este método aún no ha sido implementado.",
                SolucionSugerida = "Considere implementar la lógica necesaria para eliminar un rol existente."
            });
        }
    }
}
