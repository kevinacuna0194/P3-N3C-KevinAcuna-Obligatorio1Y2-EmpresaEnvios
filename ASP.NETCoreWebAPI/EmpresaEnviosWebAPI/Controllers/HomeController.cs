using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using EmpresaEnviosWebAPI.DTOs;
using EmpresaEnviosWebAPI.Services;
using ExcepcionesPropias;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmpresaEnviosWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> Logger;
        public IBuscarUsuarioPorEmailYPassword BuscarUsuarioPorEmailYPassword { get; set; }
        public TokenService TokenService { get; set; }

        public HomeController(ILogger<HomeController> logger, IBuscarUsuarioPorEmailYPassword buscarUsuarioPorEmailYPassword, TokenService tokenService)
        {
            Logger = logger;
            BuscarUsuarioPorEmailYPassword = buscarUsuarioPorEmailYPassword;
            TokenService = tokenService;
        }

        /// <summary>
        /// Inicia sesión en el sistema con las credenciales del usuario.
        /// </summary>
        /// <param name="credenciales">
        /// Credenciales del usuario que contiene el email y la contraseña.
        /// </param>
        /// <returns>
        /// 200 si el inicio de sesión es exitoso, junto con un token JWT y los datos del usuario.
        /// 400 si faltan campos obligatorios o si los datos son inválidos.
        /// 400 si hay un error de validación de datos.
        /// 401 si las credenciales proporcionadas no son válidas.
        /// 500 si ocurre un error interno del servidor durante el proceso de inicio de sesión. 
        /// </returns>
        // POST api/<HomeController>/IniciarSesion
        [HttpPost("IniciarSesion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult IniciarSesion([FromBody] CredencialesDTO credenciales)
        {
            try
            {
                if (string.IsNullOrEmpty(credenciales.Email) || string.IsNullOrEmpty(credenciales.Password))
                {
                    return BadRequest(new
                    {
                        Mensaje = "Todos los campos son obligatorios.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Campos Faltantes",
                        CampoFaltante = string.IsNullOrEmpty(credenciales.Email) ? "Email" : "Password",
                        Detalles = "Los campos Email y Password no pueden estar vacíos.",
                        SolucionSugerida = "Asegúrese de completar todos los datos requeridos antes de enviar la solicitud."
                    });
                }

                UsuarioDTO usuario = BuscarUsuarioPorEmailYPassword.Buscar(credenciales.Email, credenciales.Password);

                if (usuario is null)
                {
                    return Unauthorized(new
                    {
                        Mensaje = "Correo electrónico o contraseña incorrectos.",
                        Codigo = 401,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Autenticación Fallida",
                        Detalles = "No se encontró un usuario con las credenciales proporcionadas.",
                        SolucionSugerida = "Verifique su correo y contraseña, y vuelva a intentarlo."
                    });
                }

                // Generar el token JWT
                string token = TokenService.GenerarToken(new JwtDTO
                {
                    Email = usuario.Email,
                    Rol = usuario.NombreRol
                });

                return Ok(new
                {
                    Mensaje = "Inicio de sesión exitoso.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Token = token,
                    Datos = usuario
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Mensaje = ex.Message,
                    Codigo = 400,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos Inválidos",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise los datos enviados y asegúrese de que cumplan con el formato correcto."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Excepción no controlada",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Contacte al soporte técnico y proporcione detalles del error para una rápida solución."
                });
            }
        }

        /// <summary>
        /// Cierra la sesión del usuario actual y limpia la sesión.
        /// </summary>
        /// <returns>
        /// 200 si la sesión se cierra exitosamente, junto con un mensaje de confirmación y los datos del usuario.
        /// 500 si ocurre un error interno del servidor al intentar cerrar la sesión.
        /// </returns>
        // GET api/<HomeController>/CerrarSesion
        [HttpGet("CerrarSesion")]
        public IActionResult CerrarSesion()
        {
            try
            {
                // Limpiar la sesión del usuario
                HttpContext.Session.Clear();

                return Ok(new
                {
                    Mensaje = "Sesión cerrada exitosamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = new
                    {
                        Email = User.Identity?.Name,
                        Rol = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error al cerrar sesión.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Excepción no controlada",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Intente nuevamente o contacte a soporte si el problema persiste."
                });
            }
        }
    }
}
