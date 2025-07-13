using CasosUso.DTOs;
using CasosUso.InterfacesCasosUso;
using EmpresaEnviosWebAPI.DTOs;
using ExcepcionesPropias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmpresaEnviosWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public IAgregarUsuario AgregarUsuario { get; set; }
        public IActualizarUsuario ActualizarUsuario { get; set; }
        public IBuscarUsuarioPorId BuscarUsuarioPorId { get; set; }
        public IEliminarUsuario EliminarUsuario { get; set; }
        public IListaUsuarios ListaUsuarios { get; set; }
        public IListaRoles ListaRoles { get; set; }
        public IBuscarNombreRolPorId BuscarNombreRolPorId { get; set; }
        public IBuscarUsuarioPorEmail BuscarUsuarioPorEmail { get; set; }

        public UsuarioController(IAgregarUsuario agregarUsuario, IActualizarUsuario actualizarUsuario, IBuscarUsuarioPorId buscarUsuarioPorId, IEliminarUsuario eliminarUsuario, IListaUsuarios listaUsuario, IListaRoles listaRol, IBuscarNombreRolPorId buscarNombreRolPorId, IBuscarUsuarioPorEmail buscarUsuarioPorEmail)
        {
            AgregarUsuario = agregarUsuario;
            ActualizarUsuario = actualizarUsuario;
            BuscarUsuarioPorId = buscarUsuarioPorId;
            EliminarUsuario = eliminarUsuario;
            ListaUsuarios = listaUsuario;
            ListaRoles = listaRol;
            BuscarNombreRolPorId = buscarNombreRolPorId;
            BuscarUsuarioPorEmail = buscarUsuarioPorEmail;
        }

        /// <summary>
        /// Obtiene un listado de todos los usuarios registrados en el sistema.
        /// </summary>
        /// <returns>
        /// 200 si se obtienen los usuarios correctamente, junto con un mensaje de éxito y los datos de los usuarios.
        /// 400 si hay datos inválidos en la solicitud.
        /// 404 si no se encuentran usuarios registrados.
        /// 500 si ocurre un error interno del servidor al intentar obtener la lista de usuarios.
        /// </returns>
        // GET: api/<UsuarioController>/Listado
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
                IEnumerable<UsuarioDTO> usuarios = ListaUsuarios.ObtenerListaUsuarios();

                if (usuarios == null || !usuarios.Any())
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontraron usuarios registrados.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Lista de Usuarios Vacía",
                        Detalles = "Verifica que existan usuarios en el sistema y vuelve a intentarlo.",
                        SolucionSugerida = "Si eres administrador, asegúrate de haber creado usuarios previamente."
                    });
                }

                return Ok(new
                {
                    Mensaje = "Usuarios obtenidos correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = usuarios
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
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique los datos consultados y asegúrese de que existen en el sistema."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error al obtener la lista de usuarios.",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Contacte al soporte técnico y proporcione detalles del error."
                });
            }
        }

        /// <summary>
        /// Obtiene los detalles de un usuario específico por su ID.
        /// </summary>
        /// <remarsk>
        /// Utiliza el nombre de ruta "DetallesUsuario" para referenciar esta acción en otras partes de la aplicación.
        /// </remarsk>
        /// <param name="id">
        /// Identificador numérico único del usuario que se desea consultar.
        /// </param>
        /// <returns>
        /// 200 si se obtienen los detalles del usuario correctamente, junto con un mensaje de éxito y los datos del usuario.
        /// 400 si los datos proporcionados son inválidos.
        /// 404 si no se encuentra un usuario con el ID proporcionado.
        /// 500 si ocurre un error interno del servidor al intentar obtener los detalles del usuario.
        /// </returns>
        // GET api/<UsuarioController>/Detalles/5
        [HttpGet("Detalles/{id}", Name = "DetallesUsuario")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Detalles(int id)
        {
            try
            {
                UsuarioDTO usuarioDTO = BuscarUsuarioPorId.Buscar(id);

                if (usuarioDTO == null)
                {
                    return NotFound(new
                    {
                        Mensaje = $"No se encontró ningún usuario con el ID: {id}. Verifica que el ID sea correcto e inténtalo de nuevo.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario No Encontrado",
                        Detalles = $"No se encontró un usuario con el ID: {id}.",
                        SolucionSugerida = "Asegúrese de que el ID enviado es correcto y que el usuario existe en el sistema."
                    });
                }

                return Ok(new
                {
                    Mensaje = "Usuario obtenido correctamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = usuarioDTO
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
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Verifique el ID enviado y asegúrese de que sea correcto."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno en el servidor.",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    TipoError = "Error al obtener los detalles del usuario.",
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Contacte al soporte técnico y proporcione detalles del error."
                });
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="usuarioApiDTO">
        /// Objeto que contiene los datos del usuario a crear, incluyendo el ID del usuario logueado.
        /// </param>
        /// <returns>
        /// 201 si el usuario se crea exitosamente, junto con un mensaje de éxito y los datos del usuario creado.
        /// 400 si los datos proporcionados son inválidos o faltan campos requeridos.
        /// 400 si hay un error de validación de datos.
        /// 401 si el ID del usuario logueado es inválido o no se ha proporcionado.
        /// 500 si ocurre un error interno del servidor al intentar crear el usuario.
        /// </returns>
        // POST api/<UsuarioController>/Crear
        [HttpPost("Crear")]
        [Authorize(Roles = "Administrador, Funcionario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearUsuario([FromBody] UsuarioApiDTO usuarioApiDTO)
        {
            try
            {
                if (usuarioApiDTO?.usuarioDTO == null)
                {
                    return BadRequest(new
                    {
                        Mensaje = "Los datos del usuario no son válidos.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos Inválidos",
                        Detalles = "El objeto usuarioDTO no puede ser nulo.",
                        SolucionSugerida = "Asegúrese de enviar un objeto usuarioDTO con los datos requeridos."
                    });
                }

                // Validar el usuario logueado desde el DTO
                if (usuarioApiDTO.IdUsuarioLogueado <= 0)
                {
                    return Unauthorized(new
                    {
                        Mensaje = "No se pudo recuperar el ID del usuario logueado.",
                        Codigo = 401,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario No Autenticado",
                        Detalles = "El ID del usuario logueado es inválido o no se ha proporcionado.",
                        SolucionSugerida = "Asegúrese de que su sesión está activa y de que envió el ID correctamente."
                    });
                }

                // Agregar usuario al sistema
                AgregarUsuario.Agregar(usuarioApiDTO.usuarioDTO, usuarioApiDTO.IdUsuarioLogueado);

                return Created("Usuario Creado", new
                {
                    Mensaje = "Usuario registrado exitosamente.",
                    Codigo = 201,
                    Fecha = DateTime.UtcNow,
                    Datos = usuarioApiDTO.usuarioDTO
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
                    Detalles = ex.Message,
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
                    TipoError = "Error al crear el usuario.",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Intente nuevamente más tarde o contacte a soporte técnico."
                });
            }
        }

        /// <summary>
        /// Edita los datos de un usuario existente en el sistema.
        /// </summary>
        /// <param name="id">
        /// Identificador numérico único del usuario que se desea editar.
        /// </param>
        /// <param name="usuarioApiDTO">
        /// Objeto que contiene los datos del usuario a editar, incluyendo el ID del usuario logueado.
        /// </param>
        /// <returns>
        /// 200 si el usuario se edita exitosamente, junto con un mensaje de éxito y los datos del usuario editado.
        /// 400 si los datos proporcionados son inválidos o faltan campos requeridos.
        /// 400 si hay un error de validación de datos.
        /// 400 si el ID del usuario logueado es inválido o no se ha proporcionado.
        /// 404 si no se encuentra un usuario con el ID proporcionado.
        /// 500 si ocurre un error interno del servidor al intentar editar el usuario.
        /// </returns>
        // POST api/<UsuarioController>/Editar/5
        [HttpPost("Editar/{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Editar(int id, [FromBody] UsuarioApiDTO usuarioApiDTO)
        {
            try
            {
                // Validación de datos básicos
                if (usuarioApiDTO?.usuarioDTO == null || id != usuarioApiDTO.usuarioDTO.Id)
                {
                    return BadRequest(new
                    {
                        Codigo = 400,
                        Mensaje = "Los datos del usuario no son válidos o el ID no coincide.",
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos Inválidos",
                        Detalles = "El objeto usuarioDTO no puede ser nulo o el ID del usuario no coincide con el ID enviado en la URL.",
                        SolucionSugerida = "Verifique que el ID y los datos enviados sean correctos."
                    });
                }

                // Validación del usuario logueado
                if (usuarioApiDTO.IdUsuarioLogueado <= 0)
                {
                    return BadRequest(new
                    {
                        Codigo = 400,
                        Mensaje = "No se pudo recuperar el ID del usuario logueado.",
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario No Autenticado",
                        Detalles = "El ID del usuario logueado es inválido o no se ha proporcionado.",
                        SolucionSugerida = "Asegúrese de que su sesión está activa y de que envió el ID correctamente."
                    });
                }

                // Verificación de existencia del usuario antes de actualizar
                var usuarioExistente = BuscarUsuarioPorId.Buscar(id);

                if (usuarioExistente == null)
                {
                    return NotFound(new
                    {
                        Codigo = 404,
                        Mensaje = $"No se encontró un usuario con ID: {id}.",
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario No Encontrado",
                        Detalles = $"No se encontró un usuario con el ID: {id}. Verifique el ID enviado.",
                        SolucionSugerida = "Revise el ID enviado y asegúrese de que el usuario existe en el sistema."
                    });
                }

                // Actualizar usuario en el sistema
                ActualizarUsuario.Actualizar(usuarioApiDTO.usuarioDTO, usuarioApiDTO.IdUsuarioLogueado);

                return Ok(new
                {
                    Codigo = 200,
                    Mensaje = $"Usuario con ID {usuarioApiDTO.usuarioDTO.Id} editado exitosamente.",
                    Fecha = DateTime.UtcNow,
                    Datos = usuarioApiDTO.usuarioDTO
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Codigo = 400,
                    Mensaje = ex.Message,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos Inválidos",
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise los datos enviados y asegúrese de que cumplen con el formato correcto."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Codigo = 500,
                    Mensaje = "Error interno en el servidor.",
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error al actualizar el usuario.",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Intente nuevamente más tarde o contacte a soporte técnico."
                });
            }
        }

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        /// <param name="id">
        /// Identificador numérico único del usuario que se desea eliminar.
        /// </param>
        /// <param name="usuarioApiDTO">
        /// Objeto que contiene el ID del usuario logueado que está realizando la eliminación.
        /// </param>
        /// <returns>
        /// 200 si el usuario se elimina exitosamente, junto con un mensaje de éxito.
        /// 400 si los datos proporcionados son inválidos o faltan campos requeridos.
        /// 400 si hay un error de validación de datos.
        /// 404 si no se encuentra un usuario con el ID proporcionado.
        /// 500 si ocurre un error interno del servidor al intentar eliminar el usuario.
        /// </returns>
        // DELETE api/<UsuarioController>/5
        [HttpPost("Eliminar/{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Eliminar(int id, [FromBody] UsuarioApiDTO usuarioApiDTO)
        {
            try
            {
                // Validación del usuario logueado
                if (usuarioApiDTO.IdUsuarioLogueado <= 0)
                {
                    return BadRequest(new
                    {
                        Codigo = 400,
                        Mensaje = "No se pudo recuperar el ID del usuario logueado.",
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario No Autenticado",
                        Detalles = "El ID del usuario logueado es inválido o no se ha proporcionado.",
                        SolucionSugerida = "Asegúrese de que su sesión está activa y de que envió el ID correctamente."
                    });
                }

                // Verificación de existencia del usuario antes de eliminar
                var usuarioExistente = BuscarUsuarioPorId.Buscar(id);

                if (usuarioExistente == null)
                {
                    return NotFound(new
                    {
                        Codigo = 404,
                        Mensaje = $"No se encontró un usuario con ID: {id}.",
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario No Encontrado",
                        Detalles = $"No se encontró un usuario con el ID: {id}. Verifique el ID enviado.",
                        SolucionSugerida = "Verifique el ID y asegúrese de que el usuario existe en el sistema."
                    });
                }

                // Eliminar usuario
                EliminarUsuario.Eliminar(id, usuarioApiDTO.IdUsuarioLogueado);

                return Ok(new
                {
                    Codigo = 200,
                    Mensaje = $"Usuario con ID {id} eliminado exitosamente.",
                    Fecha = DateTime.UtcNow
                });
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(new
                {
                    Codigo = 400,
                    Mensaje = ex.Message,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Datos Inválidos",
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise los datos enviados y asegúrese de que cumplan con el formato correcto."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Codigo = 500,
                    Mensaje = "Error interno en el servidor.",
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error al eliminar el usuario.",
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    SolucionSugerida = "Intente nuevamente más tarde o contacte a soporte técnico."
                });
            }
        }

        /// <summary>
        /// Permite a un usuario cambiar su contraseña actual por una nueva.
        /// </summary>
        /// <param name="cambiarContraseñaDTO">
        /// Objeto que contiene los datos necesarios para cambiar la contraseña, incluyendo el email del usuario, la contraseña actual y la nueva contraseña.
        /// </param>
        /// <returns>
        /// 200 si la contraseña se cambia exitosamente, junto con un mensaje de éxito y los datos del usuario actualizado.
        /// 400 si los datos proporcionados son inválidos o faltan campos requeridos.
        /// 400 si hay un error de validación de datos.
        /// 401 si el ID del usuario logueado es inválido o no se ha proporcionado.
        /// 404 si no se encuentra un usuario con el email proporcionado.
        /// 500 si ocurre un error interno del servidor al intentar cambiar la contraseña del usuario.
        /// </returns>
        // POST api/<UsuarioController>/CambiarContraseña
        [HttpPost("CambiarContraseña")]
        [Authorize(Roles = "Administrador, Funcionario, Cliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CambiarContraseña([FromBody] CambiarContraseñaDTO cambiarContraseñaDTO)
        {
            try
            {
                if (cambiarContraseñaDTO == null)
                {
                    return BadRequest(new
                    {
                        Mensaje = "Los datos de cambio de contraseña no son válidos.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Datos Inválidos",
                        Detalles = "El objeto CambiarContraseñaDTO no puede ser nulo.",
                        SolucionSugerida = "Asegúrese de enviar un objeto CambiarContraseñaDTO con los datos requeridos."
                    });
                }

                // Validar el usuario logueado desde el DTO
                if (cambiarContraseñaDTO.IdUsuarioLogueado <= 0)
                {
                    return Unauthorized(new
                    {
                        Mensaje = "No se pudo recuperar el ID del usuario logueado.",
                        Codigo = 401,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario No Autenticado",
                        Detalles = "El ID del usuario logueado es inválido o no se ha proporcionado.",
                        SolucionSugerida = "Asegúrese de que su sesión está activa y de que envió el ID correctamente."
                    });
                }

                // Buscar usuario por email y contraseña actual
                UsuarioDTO usuarioDTO = BuscarUsuarioPorEmail.Buscar(cambiarContraseñaDTO.Email);

                if (usuarioDTO is null)
                {
                    return NotFound(new
                    {
                        Mensaje = "No se encontró un usuario con el correo electrónico proporcionado.",
                        Codigo = 404,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Usuario No Encontrado",
                        Detalles = "El correo electrónico proporcionado no corresponde a ningún usuario registrado.",
                        SolucionSugerida = "Verifique su correo electrónico e inténtelo de nuevo."
                    });
                }

                if (usuarioDTO.Password != cambiarContraseñaDTO.ContraseñaActual)
                {
                    return BadRequest(new
                    {
                        Mensaje = "La contraseña actual es incorrecta.",
                        Codigo = 400,
                        FechaError = DateTime.UtcNow,
                        TipoError = "Validación de Contraseña",
                        Detalles = "La contraseña actual proporcionada no coincide con la registrada.",
                        SolucionSugerida = "Verifique su contraseña actual e inténtelo de nuevo."
                    });
                }

                // Actualizar la contraseña del usuario
                usuarioDTO.Password = cambiarContraseñaDTO.NuevaContraseña;

                // Cambiar contraseña del usuario
                ActualizarUsuario.Actualizar(usuarioDTO, cambiarContraseñaDTO.IdUsuarioLogueado);

                return Ok(new
                {
                    Mensaje = "Contraseña cambiada exitosamente.",
                    Codigo = 200,
                    Fecha = DateTime.UtcNow,
                    Datos = usuarioDTO
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
                    Detalles = ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Revise los datos enviados y asegúrese de que cumplan con el formato correcto."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = "Error interno en el servidor",
                    Codigo = 500,
                    FechaError = DateTime.UtcNow,
                    TipoError = "Error al cambiar la contraseña",
                    Detalles = ex.InnerException?.Message ?? ex.Message,
                    StackTrace = ex.InnerException?.StackTrace ?? ex.StackTrace,
                    SolucionSugerida = "Intente nuevamente más tarde o contacte a soporte técnico"
                });
            }
        }
    }
}
