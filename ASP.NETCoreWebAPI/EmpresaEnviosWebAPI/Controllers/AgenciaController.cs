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
    public class AgenciaController : ControllerBase
    {
        public IListaAgencias ListaAgencias { get; set; }
        public IBuscarAgenciaPorId BuscarAgenciaPorId { get; set; }

        public AgenciaController(IListaAgencias listaAgencias, IBuscarAgenciaPorId buscarAgenciaPorId)
        {
            ListaAgencias = listaAgencias;
            BuscarAgenciaPorId = buscarAgenciaPorId;
        }

        /// <summary>
        /// Obtiene un listado de todas las agencias registradas en el sistema.
        /// </summary>
        /// <returns>
        /// 200 si se obtienen las agencias correctamente, junto con un mensaje de éxito y los datos de las agencias.
        /// 400 si hay datos inválidos en la solicitud.
        /// 404 si no se encuentran agencias registradas.
        /// 500 si ocurre un error interno del servidor.
        /// </returns>
        // GET: api/<AgenciaController/Listado>
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
                IEnumerable<AgenciaDTO> agencias = ListaAgencias.ObtenerListaAgencias();

                if (agencias is null || !agencias.Any())
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontraron agencias registradas.",
                        Codigo = 404,
                        Fecha = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = "No se encontraron agencias registradas en el sistema.",
                        SolucionSugerida = "Registre al menos una agencia antes de intentar recuperar los datos."
                    });
                }

                return Ok(new
                {
                    Mensaje = "Agencias obtenidas correctamente",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    TotalAgencias = agencias.Count(),
                    Datos = agencias
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Error = "Datos inválidos en la solicitud.",
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos no válidos",
                    Detalles = ex.Message,
                    StackTrace = ex.StackTrace,
                    SolucionSugerida = "Registre al menos una agencia antes de intentar recuperar los datos."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Error interno del servidor",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error inesperado",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.StackTrace,
                    SolucionSugerida = "Revise la excepción interna y valide los datos antes de reenviar la solicitud."
                });
            }
        }

        /// <summary>
        /// Obtiene los detalles de una agencia específica por su ID.
        /// </summary>
        /// <remarks>
        /// Esta acción puede ser referenciada mediante el nombre de ruta "DetallesAgencia" para facilitar su uso en otras partes de la aplicación. 
        /// </remarks>
        /// <param name="id">Identificador numérico único de la agencia.</param>
        /// <returns>
        /// 200 si se encuentran los detalles de la agencia correctamente, junto con un mensaje de éxito y los datos de la agencia.
        /// 400 si los datos proporcionados son inválidos.
        /// 404 si no se encuentra una agencia con el ID proporcionado.
        /// 500 si ocurre un error interno del servidor.
        /// </returns>
        // GET api/<AgenciaController>/Detalles/5
        [HttpGet("Detalles/{id}", Name = "DetallesAgencia")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Detalles(int id)
        {
            try
            {
                AgenciaDTO agencia = BuscarAgenciaPorId.Buscar(id);

                if (agencia == null)
                {
                    return NotFound(new
                    {
                        Mensaje = "Agencia no encontrada.",
                        Codigo = 404,
                        Fecha = DateTime.UtcNow,
                        TipoError = "Datos no encontrados",
                        Detalles = "No se encontró una agencia con el ID proporcionado.",
                        SolucionSugerida = "Verifique el ID de la agencia y asegúrese de que exista en el sistema."
                    });
                }

                return Ok(new
                {
                    Mensaje = "Operación exitosa. Detalles de la agencia recuperados correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = agencia
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
                    StackTrace = ex.StackTrace,
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

        // POST api/<AgenciaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AgenciaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AgenciaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
