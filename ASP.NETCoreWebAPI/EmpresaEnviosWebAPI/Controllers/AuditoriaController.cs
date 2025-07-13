using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using ExcepcionesPropias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmpresaEnviosWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        public IListaAuditorias ListaAuditorias { get; set; }
        public IBuscarAuditoriaPorId BuscarAuditoriaPorId { get; set; }

        public AuditoriaController(IListaAuditorias listaAuditoria, IBuscarAuditoriaPorId buscarAuditoriaPorId)
        {
            ListaAuditorias = listaAuditoria;
            BuscarAuditoriaPorId = buscarAuditoriaPorId;
        }

        /// <summary>
        /// Obtiene un listado de todas las auditorías registradas en el sistema.
        /// </summary>
        /// <returns>
        /// 200 si se obtienen las auditorías correctamente, junto con un mensaje de éxito y los datos de las auditorías.
        /// 400 si hay datos inválidos en la solicitud.
        /// 404 si no se encuentran auditorías registradas.
        /// 500 si ocurre un error interno del servidor.
        /// </returns>
        [HttpGet("Listado")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Listado()
        {
            try
            {
                IEnumerable<AuditoriaDTO> auditorias = ListaAuditorias.ObtenerListaAuditorias();

                if (!auditorias.Any())
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontraron auditorías.",
                        Codigo = 404,
                        Fecha = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = "No se encontraron registros de auditoría en el sistema.",
                        SolucionSugerida = "Registre al menos una auditoría antes de intentar recuperar los datos."
                    });
                }

                return Ok(new
                {
                    Mensaje = "Operación exitosa. Datos de auditoría recuperados correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = auditorias
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
        /// Obtiene los detalles de una auditoría específica por su ID.
        /// </summary>
        /// <remarks>
        /// Esta accion puede ser referenciada mediante el nombre de ruta "DetallesAuditoria" para facilitar su uso en otras partes de la aplicación.
        /// </remarks>
        /// <param name="id">
        /// Identificador numérico único de la auditoría.
        /// </param>
        /// <returns>
        /// 200 si se encuentran los detalles de la auditoría correctamente, junto con un mensaje de éxito y los datos de la auditoría.
        /// 400 si los datos proporcionados son inválidos.
        /// 404 si no se encuentra una auditoría con el ID proporcionado.
        /// 500 si ocurre un error interno del servidor.
        /// </returns>
        // GET api/<AuditoriaController>/Detalles/5
        [HttpGet("Detalles/{id}", Name = "DetallesAuditoria")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Detalles(int id)
        {
            try
            {
                AuditoriaDTO auditoria = BuscarAuditoriaPorId.Buscar(id);

                if (auditoria == null)
                {
                    return NotFound(new
                    {
                        Mensaje = "Auditoría no encontrada.",
                        Codigo = 404,
                        Fecha = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = "No se encontró un registro de auditoría con el ID proporcionado.",
                        SolucionSugerida = "Verifique el ID proporcionado y asegúrese de que la auditoría exista en el sistema."
                    });
                }

                return Ok(new
                {
                    Mensaje = "Operación exitosa. Detalles de la auditoría recuperados correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = auditoria
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
        
        [HttpPost("Crear")]
        [ProducesResponseType(statusCode: StatusCodes.Status501NotImplemented)]
        public IActionResult Crear([FromBody] AuditoriaDTO auditoriaDTO)
        {
            return StatusCode(501, new
            {
                Mensaje = "La acción 'Crear' no está implementada.",
                Codigo = 501,
                FechaError = DateTime.UtcNow,
                TipoError = "Funcionalidad no implementada",
                Detalles = "La funcionalidad para crear una nueva auditoría aún no está implementada.",
                StackTrace = new StackTrace().ToString(),
                SolucionSugerida = "Consulte la documentación para futuras implementaciones."
            });
        }

        // PUT api/<AuditoriaController>/Editar/5
        [HttpPut("Editar/{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status501NotImplemented)]
        public IActionResult Editar(int id, [FromBody] string value)
        {
            return StatusCode(501, new
            {
                Mensaje = "La acción 'Editar' no está implementada.",
                Codigo = 501,
                FechaError = DateTime.UtcNow,
                TipoError = "Funcionalidad no implementada",
                Detalles = "La funcionalidad para crear una nueva auditoría aún no está implementada.",
                StackTrace = new StackTrace().ToString(),
                SolucionSugerida = "Consulte la documentación para futuras implementaciones."
            });
        }

        // DELETE api/<AuditoriaController>/Eliminar/5
        [HttpDelete("Eliminar/{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status501NotImplemented)]
        public IActionResult Eliminar(int id)
        {
            return StatusCode(501, new
            {
                Mensaje = "La acción 'Eliminar' no está implementada.",
                Codigo = 501,
                FechaError = DateTime.UtcNow,
                TipoError = "Funcionalidad no implementada",
                Detalles = "La funcionalidad para crear una nueva auditoría aún no está implementada.",
                StackTrace = new StackTrace().ToString(),
                SolucionSugerida = "Consulte la documentación para futuras implementaciones."
            });
        }
    }
}
