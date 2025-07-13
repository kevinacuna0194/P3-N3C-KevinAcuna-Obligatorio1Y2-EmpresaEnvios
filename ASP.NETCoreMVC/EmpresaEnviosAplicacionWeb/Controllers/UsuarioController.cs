using DTOs;
using EmpresaEnviosAplicacionWeb.Models;
using Exceptions;
using Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Presentacion.Controllers
{
    public class UsuarioController : Controller
    {

        public ApiService ApiService { get; set; }
        public HttpClientService HttpClientService { get; set; }
        public RolService RolService { get; set; }
        public UsuarioService UsuarioService { get; set; }
        public NombreRolPorIdService NombreRolPorIdService { get; set; }

        public UsuarioController(ApiService apiService, HttpClientService httpClientService, RolService rolService, UsuarioService usuarioService, NombreRolPorIdService nombreRolPorIdService)
        {
            ApiService = apiService;
            HttpClientService = httpClientService;
            RolService = rolService;
            UsuarioService = usuarioService;
            NombreRolPorIdService = nombreRolPorIdService;
        }

        // GET: UsuarioController
        [ValidarSesion]
        public async Task<ActionResult> Listado()
        {
            try
            {
                // Utilizar el servicio para obtener la URL del endpoint de Usuario
                string url = ApiService.GetEndpointUrl("Usuario", "Listado");

                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("La URL del servicio de Usuario no está configurada correctamente");
                }

                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud GET al servicio de Usuario
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Get, token);

                // Verificar si la respuesta fue exitosa
                if (!respuesta.IsSuccessStatusCode)
                {
                    string contenido = await respuesta.Content.ReadAsStringAsync();
                    ErrorApiDTO error = null;

                    try
                    {
                        error = JsonConvert.DeserializeObject<ErrorApiDTO>(contenido);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"[Error API] Fallo en la deserialización del JSON: {ex.Message}");
                    }

                    var codigo = error?.Codigo ?? (int)respuesta.StatusCode;
                    var mensaje = error?.Mensaje ?? "Error desconocido al obtener listado de usuarios.";
                    var detalles = error?.Detalles ?? contenido;
                    var sugerencia = error?.SolucionSugerida ?? "Intente recargar la página o consulte con el administrador.";
                    var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

                    var errorModel = new ErrorViewModel
                    {
                        Codigo = codigo,
                        Mensaje = mensaje,
                        Detalles = detalles,
                        Sugerencia = sugerencia,
                        RequestId = requestId
                    };

                    Console.WriteLine($"[Error API] Código: {codigo}, Mensaje: {mensaje}, RequestId: {requestId}");

                    return View("Error", errorModel);
                }

                // Obtener el cuerpo de la respuesta
                var body = await HttpClientService.ObtenerBodyAsync(respuesta);

                if (string.IsNullOrEmpty(body))
                {
                    TempData["Error"] = "No se encontraron usuarios registrados.";
                    return RedirectToAction(nameof(Listado));
                }

                // Deserializar el cuerpo de la respuesta a una lista de UsuarioDTO
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<List<UsuarioDTO>>>(body);

                if (resultado == null || resultado.Datos == null)
                {
                    TempData["Error"] = "No se encontraron usuarios registrados.";
                    return RedirectToAction(nameof(Listado));
                }

                IEnumerable<UsuarioDTO> listaUsuarios = resultado.Datos;

                return View(listaUsuarios);
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message}. Contacte a soporte." :
                    $"Se produjo un error: {ex.Message}. Contacte a soporte.";
            }

            return RedirectToAction(nameof(Listado));
        }

        // GET: UsuarioController/Detalles/5
        [ValidarSesion]
        public async Task<ActionResult> Detalles(int id)
        {
            try
            {
                // Validar que el ID del usuario sea válido
                string url = ApiService.GetEndpointUrl("Usuario", $"Detalles/{id}");

                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException(nameof(url), "La URL del servicio de Usuario no está configurada correctamente.");
                }

                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud GET al servicio de Usuario para obtener los detalles del usuario
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Get, token);

                // Verificar si la respuesta fue exitosa
                if (!respuesta.IsSuccessStatusCode)
                {
                    string contenido = await respuesta.Content.ReadAsStringAsync();
                    ErrorApiDTO error = null;

                    try
                    {
                        error = JsonConvert.DeserializeObject<ErrorApiDTO>(contenido);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"[Error API] Fallo en la deserialización del JSON: {ex.Message}");
                    }

                    var codigo = error?.Codigo ?? (int)respuesta.StatusCode;
                    var mensaje = error?.Mensaje ?? "Error desconocido al obtener detalles del usuario.";
                    var detalles = error?.Detalles ?? contenido;
                    var sugerencia = error?.SolucionSugerida ?? "Intente recargar la página o consulte con el administrador.";
                    var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

                    var errorModel = new ErrorViewModel
                    {
                        Codigo = codigo,
                        Mensaje = mensaje,
                        Detalles = detalles,
                        Sugerencia = sugerencia,
                        RequestId = requestId
                    };

                    Console.WriteLine($"[Error API] Código: {codigo}, Mensaje: {mensaje}, RequestId: {requestId}");

                    return View("Error", errorModel);
                }

                // Obtener el cuerpo de la respuesta
                var body = await HttpClientService.ObtenerBodyAsync(respuesta);

                if (string.IsNullOrEmpty(body))
                {
                    throw new ArgumentNullException(nameof(body), $"No se encontraron detalles para el usuario con ID {id}.");
                }

                // Deserializar respuesta JSON a UsuarioDTO
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<UsuarioDTO>>(body);

                if (resultado == null || resultado.Datos == null)
                {
                    throw new DatosInvalidosException($"No se encontraron detalles para el usuario con ID {id}.");
                }

                UsuarioDTO usuarioDTO = resultado.Datos;

                return View(usuarioDTO);
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message}. Contacte a soporte." :
                    $"Se produjo un error: {ex.Message}. Contacte a soporte.";
            }

            return RedirectToAction(nameof(Listado));
        }

        // GET: UsuarioController/Crear
        [ValidarSesion]
        public async Task<ActionResult> Crear()
        {
            try
            {
                IEnumerable<RolDTO> roles = await RolService.ObtenerRolesAsync();

                UsuarioVM UsuarioVM = new UsuarioVM()
                {
                    UsuarioDTO = new UsuarioDTO(),
                    Roles = roles
                };

                return View(UsuarioVM);
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message} Contacte a soporte." :
                    $"Se produjo un error: {ex.Message} Contacte a soporte.";
            }

            return View();
        }

        // POST: UsuarioController/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(UsuarioVM usuarioVM)
        {
            try
            {
                // Utilizar el servicio para obtener la URL del endpoint de Usuario
                string url = ApiService.GetEndpointUrl("Usuario", "Crear");

                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException(nameof(url), "La URL del servicio de Usuario no está configurada correctamente.");
                }

                // Obtener el NombreRol del RolId seleccionado
                usuarioVM.UsuarioDTO.NombreRol = await NombreRolPorIdService.NombreRolAsync(usuarioVM.UsuarioDTO.RolId);

                var usuarioApiDTO = new UsuarioApiDTO
                {
                    usuarioDTO = usuarioVM.UsuarioDTO,
                    RolId = usuarioVM.UsuarioDTO.RolId,
                    IdUsuarioLogueado = HttpContext.Session.GetInt32("Id") ?? 0
                };

                // Obtener el token de sesión
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud POST al servicio de Usuario para crear un nuevo usuario
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, usuarioApiDTO, token);

                // Verificar si la respuesta fue exitosa
                if (!respuesta.IsSuccessStatusCode)
                {
                    string contenido = await respuesta.Content.ReadAsStringAsync();
                    ErrorApiDTO error = null;

                    try
                    {
                        error = JsonConvert.DeserializeObject<ErrorApiDTO>(contenido);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"[Error API] Fallo en la deserialización del JSON: {ex.Message}");
                    }

                    var codigo = error?.Codigo ?? (int)respuesta.StatusCode;
                    var mensaje = error?.Mensaje ?? "Error desconocido al crear el usuario.";
                    var detalles = error?.Detalles ?? contenido;
                    var sugerencia = error?.SolucionSugerida ?? "Intente recargar la página o consulte con el administrador.";
                    var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

                    var errorModel = new ErrorViewModel
                    {
                        Codigo = codigo,
                        Mensaje = mensaje,
                        Detalles = detalles,
                        Sugerencia = sugerencia,
                        RequestId = requestId
                    };

                    Console.WriteLine($"[Error API] Código: {codigo}, Mensaje: {mensaje}, RequestId: {requestId}");

                    return View("Error", errorModel);
                }

                // Obtener el cuerpo de la respuesta
                var body = await HttpClientService.ObtenerBodyAsync(respuesta);

                if (string.IsNullOrEmpty(body))
                {
                    throw new ArgumentNullException(nameof(body), "No se pudo obtener la respuesta del servidor al crear el usuario.");
                }

                // Deserializar respuesta JSON a UsuarioDTO
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<UsuarioDTO>>(body);

                if (resultado == null || resultado.Datos == null)
                {
                    throw new DatosInvalidosException("No se pudo crear el usuario. Verifica los datos e inténtalo de nuevo.");
                }

                // Asignar el usuario creado al modelo de vista
                usuarioVM.UsuarioDTO = resultado.Datos;

                TempData["Exito"] = $"Usuario registrado exitosamente. <mark class='bg-warning text-dark rounded-pill px-2 py-1'>Nombre: {usuarioVM.UsuarioDTO.Nombre}</mark>. <mark class='bg-success text-white rounded-pill px-2 py-1'>Email: {usuarioVM.UsuarioDTO.Email}</mark>";

                if (HttpContext.Session.GetString("Rol") == "Administrador")
                {
                    return RedirectToAction(nameof(Listado));
                }
                else if (HttpContext.Session.GetString("Rol") == "Funcionario")
                {
                    return RedirectToAction("Listado", "Envio");
                }
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message}. Contacte a soporte." :
                    $"Se produjo un error: {ex.Message}. Contacte a soporte.";
            }

            // Si hay un error, se recarga la lista de roles antes de volver a la vista
            usuarioVM.Roles = await RolService.ObtenerRolesAsync();

            return View(usuarioVM);
        }

        // GET: UsuarioController/Editar/5
        [HttpGet]
        [ValidarSesion]
        public async Task<ActionResult> Editar(int id)
        {
            try
            {
                UsuarioDTO usuarioDTO = await UsuarioService.ObtenerUsuarioAsync(id);

                if (usuarioDTO == null)
                {
                    throw new DatosInvalidosException($"No se encontró ningún usuario con el ID: {id}. Verifica que el ID sea correcto e inténtalo de nuevo.");
                }

                IEnumerable<RolDTO> roles = await RolService.ObtenerRolesAsync();

                if (roles == null || !roles.Any())
                {
                    throw new DatosInvalidosException("No se encontraron roles disponibles para asignar al usuario.");
                }

                UsuarioVM usuarioVM = new UsuarioVM
                {
                    UsuarioDTO = usuarioDTO,
                    Roles = roles
                };

                return View(usuarioVM);
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message}. Contacte a soporte." :
                    $"Se produjo un error: {ex.Message}. Contacte a soporte.";
            }

            return View();
        }

        // POST: UsuarioController/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(int id, UsuarioVM usuarioVM)
        {
            try
            {
                if (id <= 0)
                {
                    throw new DatosInvalidosException("El ID del usuario es inválido.");
                }

                // Obtener el NombreRol del RolId seleccionado
                string nombreRol = await NombreRolPorIdService.NombreRolAsync(usuarioVM.UsuarioDTO.RolId);

                if (string.IsNullOrEmpty(nombreRol))
                {
                    throw new DatosInvalidosException("El rol seleccionado no es válido o no existe.");
                }

                // Validar que el usuarioVM y su UsuarioDTO no sean nulos
                if (usuarioVM?.UsuarioDTO == null)
                {
                    throw new DatosInvalidosException("Los datos del usuario no son válidos.");
                }

                // Asignar el NombreRol al UsuarioDTO
                usuarioVM.UsuarioDTO.NombreRol = nombreRol;

                // Obtener el ID del usuario logueado desde la sesión
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("Id");

                if (!idUsuarioLogueado.HasValue)
                {
                    TempData["Error"] = "No se pudo recuperar el ID del usuario logueado.";
                    return RedirectToAction("Login", "Home");
                }

                // Utilizar el servicio para obtener la URL del endpoint de Usuario
                string url = ApiService.GetEndpointUrl("Usuario", $"Editar/{id}");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Usuario no está configurada correctamente.");
                }

                // Crear el objeto UsuarioApiDTO con los datos necesarios
                var usuarioApiDTO = new UsuarioApiDTO
                {
                    usuarioDTO = usuarioVM.UsuarioDTO,
                    IdUsuarioLogueado = idUsuarioLogueado.Value,
                    RolId = usuarioVM.UsuarioDTO.RolId
                };

                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud POST al servicio de Usuario para editar el usuario
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, usuarioApiDTO, token);

                // Verificar si la respuesta fue exitosa
                if (!respuesta.IsSuccessStatusCode)
                {
                    string contenido = await respuesta.Content.ReadAsStringAsync();
                    ErrorApiDTO error = null;

                    try
                    {
                        error = JsonConvert.DeserializeObject<ErrorApiDTO>(contenido);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"[Error API] Fallo en la deserialización del JSON: {ex.Message}");
                    }

                    var codigo = error?.Codigo ?? (int)respuesta.StatusCode;
                    var mensaje = error?.Mensaje ?? "Error desconocido al editar el usuario.";
                    var detalles = error?.Detalles ?? contenido;
                    var sugerencia = error?.SolucionSugerida ?? "Intente recargar la página o consulte con el administrador.";
                    var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

                    var errorModel = new ErrorViewModel
                    {
                        Codigo = codigo,
                        Mensaje = mensaje,
                        Detalles = detalles,
                        Sugerencia = sugerencia,
                        RequestId = requestId
                    };

                    Console.WriteLine($"[Error API] Código: {codigo}, Mensaje: {mensaje}, RequestId: {requestId}");

                    return View("Error", errorModel);
                }

                // Obtener el cuerpo de la respuesta
                var body = await HttpClientService.ObtenerBodyAsync(respuesta);

                if (string.IsNullOrEmpty(body))
                {
                    throw new ArgumentNullException(nameof(body), "No se pudo obtener la respuesta del servidor al editar el usuario.");
                }

                // Deserializar respuesta JSON a UsuarioDTO
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<UsuarioDTO>>(body);

                if (resultado == null || resultado.Datos == null)
                {
                    throw new DatosInvalidosException("No se pudo editar el usuario. Verifica los datos e inténtalo de nuevo.");
                }

                // Asignar el usuario editado al modelo de vista
                usuarioVM.UsuarioDTO = resultado.Datos;

                TempData["Exito"] = $"Usuario con <mark class='bg-success text-white rounded-pill px-2 py-1'>ID: {usuarioVM.UsuarioDTO.Id}</mark> editado exitosamente";

                return RedirectToAction(nameof(Listado));
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message}. Contacte a soporte." :
                    $"Error inesperado: {ex.Message}. Contacte a soporte.";
            }

            // Si hay un error, volvemos a cargar la vista de edición con los roles disponibles
            usuarioVM.Roles = await RolService.ObtenerRolesAsync();

            return View(usuarioVM);
        }

        // GET: UsuarioController/Eliminar/5
        [ValidarSesion]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                UsuarioDTO usuario = await UsuarioService.ObtenerUsuarioAsync(id);

                if (usuario == null)
                {
                    throw new DatosInvalidosException($"No se encontró ningún usuario con el ID: {id}.Verifica que el ID sea correcto e inténtalo de nuevo.");
                }

                return View(usuario);
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message}. Contacte a soporte." :
                    $"Se produjo un error: {ex.Message}. Contacte a soporte.";
            }

            return View();
        }

        // DELETE: UsuarioController/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Eliminar(int id, UsuarioDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO == null || id <= 0)
                {
                    throw new DatosInvalidosException("Los datos del usuario no son válidos o el ID es incorrecto.");
                }

                // Obtenr el usuario utilizando el servicio para verificar su existencia
                usuarioDTO = await UsuarioService.ObtenerUsuarioAsync(id);

                if (usuarioDTO == null)
                {
                    throw new DatosInvalidosException($"No se encontró ningún usuario con el ID: {id}. Verifica que el ID sea correcto e inténtalo de nuevo.");
                }

                // Utilizar el servicio para obtener la URL del endpoint de Usuario
                string url = ApiService.GetEndpointUrl("Usuario", $"Eliminar/{id}");

                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException(nameof(url), "La URL del servicio de Usuario no está configurada correctamente.");
                }

                // Obtener el ID del usuario logueado desde la sesión
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("Id");

                if (!idUsuarioLogueado.HasValue)
                {
                    TempData["Error"] = "No se pudo recuperar el ID del usuario logueado.";
                    return RedirectToAction("Login", "Home");
                }

                // Crear el objeto UsuarioApiDTO con los datos necesarios
                UsuarioApiDTO usuarioApiDTO = new UsuarioApiDTO
                {
                    usuarioDTO = usuarioDTO,
                    IdUsuarioLogueado = idUsuarioLogueado.Value,
                    RolId = usuarioDTO.RolId
                };

                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud POST al servicio de Usuario para eliminar el usuario
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, usuarioApiDTO, token);

                // Verificar si la respuesta fue exitosa
                if (!respuesta.IsSuccessStatusCode)
                {
                    string contenido = await respuesta.Content.ReadAsStringAsync();
                    ErrorApiDTO error = null;

                    try
                    {
                        error = JsonConvert.DeserializeObject<ErrorApiDTO>(contenido);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"[Error API] Fallo en la deserialización del JSON: {ex.Message}");
                    }

                    var codigo = error?.Codigo ?? (int)respuesta.StatusCode;
                    var mensaje = error?.Mensaje ?? "Error desconocido al eliminar el usuario.";
                    var detalles = error?.Detalles ?? contenido;
                    var sugerencia = error?.SolucionSugerida ?? "Intente recargar la página o consulte con el administrador.";
                    var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

                    var errorModel = new ErrorViewModel
                    {
                        Codigo = codigo,
                        Mensaje = mensaje,
                        Detalles = detalles,
                        Sugerencia = sugerencia,
                        RequestId = requestId
                    };

                    Console.WriteLine($"[Error API] Código: {codigo}, Mensaje: {mensaje}, RequestId: {requestId}");

                    return View("Error", errorModel);
                }

                TempData["Exito"] = $"Usuario con <mark class='bg-success text-white rounded-pill px-2 py-1'>ID: {id}</mark> eliminado exitosamente";

                return RedirectToAction(nameof(Listado));
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message} Contacte a soporte." :
                    $"Se produjo un error: {ex.Message} Contacte a soporte.";
            }

            // Si hay un error, volvemos a cargar la vista de eliminación
            return View();
        }

        // GET: UsuarioController/CambiarContraseña
        [HttpGet]
        [ValidarSesion]
        public ActionResult CambiarContraseña()
        {
            try
            {
                CambiarContraseñaVM cambiarContraseñaVM = new CambiarContraseñaVM();

                return View(cambiarContraseñaVM);
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message}. Contacte a soporte." :
                    $"Se produjo un error: {ex.Message}. Contacte a soporte.";
            }

            return View();
        }

        // POST: UsuarioController/CambiarContraseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CambiarContraseña(CambiarContraseñaVM cambiarContraseñaVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new DatosInvalidosException("Los datos ingresados no son válidos. Por favor, verifica los campos e intenta nuevamente.");
                }

                // Validar que las contraseñas sean iguales
                if (cambiarContraseñaVM.NuevaPassword != cambiarContraseñaVM.ConfirmarPassword)
                {
                    throw new DatosInvalidosException("Las contraseñas nuevas no coinciden. Por favor, verifica e intenta nuevamente.");
                }

                string email = HttpContext.Session.GetString("Email");

                if (string.IsNullOrEmpty(email))
                {
                    throw new DatosInvalidosException("No se pudo recuperar el email del usuario logueado. Por favor, inicia sesión nuevamente.");
                }

                int idUsuarioLogueado = HttpContext.Session.GetInt32("Id").Value;

                if (idUsuarioLogueado <= 0)
                {
                    throw new DatosInvalidosException("El ID del usuario logueado es inválido. Por favor, inicia sesión nuevamente.");
                }

                CambiarContraseñaDTO cambiarContraseñaDTO = new CambiarContraseñaDTO
                {
                    Email = email,
                    ContraseñaActual = cambiarContraseñaVM.ActualPassword,
                    NuevaContraseña = cambiarContraseñaVM.NuevaPassword,
                    IdUsuarioLogueado = idUsuarioLogueado
                };

                // Utilizar el servicio para obtener la URL del endpoint de Usuario
                string url = ApiService.GetEndpointUrl("Usuario", "CambiarContraseña");

                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException(nameof(url), "La URL del servicio de Usuario no está configurada correctamente.");
                }

                // Obtener el token de sesión
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud POST al servicio de Usuario para cambiar la contraseña
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, cambiarContraseñaDTO, token);

                // Verificar si la respuesta fue exitosa
                if (!respuesta.IsSuccessStatusCode)
                {
                    string contenido = await respuesta.Content.ReadAsStringAsync();
                    ErrorApiDTO error = null;

                    try
                    {
                        error = JsonConvert.DeserializeObject<ErrorApiDTO>(contenido);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"[Error API] Fallo en la deserialización del JSON: {ex.Message}");
                    }

                    var codigo = error?.Codigo ?? (int)respuesta.StatusCode;
                    var mensaje = error?.Mensaje ?? "Error desconocido al cambiar la contraseña.";
                    var detalles = error?.Detalles ?? contenido;
                    var sugerencia = error?.SolucionSugerida ?? "Intente recargar la página o consulte con el administrador.";
                    var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

                    var errorModel = new ErrorViewModel
                    {
                        Codigo = codigo,
                        Mensaje = mensaje,
                        Detalles = detalles,
                        Sugerencia = sugerencia,
                        RequestId = requestId
                    };

                    Console.WriteLine($"[Error API] Código: {codigo}, Mensaje: {mensaje}, RequestId: {requestId}");

                    return View("Error", errorModel);
                }

                TempData["Exito"] = "Contraseña cambiada exitosamente. Por favor, inicia sesión nuevamente con tu nueva contraseña.";

                return RedirectToAction("CerrarSesion", "Home");
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null ?
                    $"Error interno: {ex.InnerException.Message} Contacte a soporte." :
                    $"Se produjo un error: {ex.Message} Contacte a soporte.";
            }

            // Si hay un error, volvemos a cargar la vista de cambio de contraseña
            return View(cambiarContraseñaVM);
        }
    }
}