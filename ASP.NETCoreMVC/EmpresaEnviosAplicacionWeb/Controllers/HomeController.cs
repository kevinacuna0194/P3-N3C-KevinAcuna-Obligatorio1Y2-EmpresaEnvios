using DTOs;
using EmpresaEnviosAplicacionWeb.Models;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using System.Diagnostics;

namespace Presentacion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> Logger;
        public ApiService ApiService { get; set; }
        public HttpClientService HttpClientService { get; set; }

        public HomeController(ILogger<HomeController> logger, ApiService apiService, HttpClientService httpClientService)
        {
            Logger = logger;
            ApiService = apiService;
            HttpClientService = httpClientService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    TempData["Error"] = "Email y contraseña son obligatorios";
                    return RedirectToAction(nameof(Login));
                }

                CredencialesDTO credencialesDTO = new CredencialesDTO
                {
                    Email = email,
                    Password = password
                };

                string url = ApiService.GetEndpointUrl("Home", "IniciarSesion");

                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("La URL del servicio de Usuario no está configurada correctamente");
                }

                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, credencialesDTO);

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
                    var mensaje = error?.Mensaje ?? "Error desconocido al iniciar sesión.";
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

                var body = await HttpClientService.ObtenerBodyAsync(respuesta);

                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<UsuarioDTO>>(body);

                if (resultado is null)
                {
                    TempData["Error"] = "Error al procesar la respuesta del servidor";
                    return RedirectToAction(nameof(Login));
                }

                UsuarioDTO usuario = resultado.Datos;

                if (usuario is not null)
                {
                    HttpContext.Session.SetString("Token", resultado.Token);
                    HttpContext.Session.SetInt32("Id", usuario.Id);
                    HttpContext.Session.SetString("Nombre", usuario.Nombre);
                    HttpContext.Session.SetString("Email", usuario.Email);
                    HttpContext.Session.SetString("Rol", usuario.NombreRol);

                    return usuario.NombreRol switch
                    {
                        "Administrador" => RedirectToAction("Listado", "Usuario"),
                        "Funcionario" => RedirectToAction("Listado", "Envio"),
                        "Cliente" => RedirectToAction("ListadoCliente", "Envio"),
                        _ => throw new DatosInvalidosException("Acceso denegado: el rol asignado a este usuario no tiene permiso para iniciar sesión. Si requiere acceso, comuníquese con el administrador del sistema.")
                    };
                }

                TempData["Error"] = "Error al procesar la respuesta";
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null
                    ? $"Error interno: {ex.InnerException.Message} Contacte a soporte."
                    : $"Se produjo un error: {ex.Message} Contacte a soporte.";
            }

            ViewBag.Email = email;
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<ActionResult> CerrarSesion()
        {
            try
            {
                string url = ApiService.GetEndpointUrl("Home", "CerrarSesion");

                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("La URL del servicio de Usuario no está configurada correctamente");
                }

                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Get);

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
                    var mensaje = error?.Mensaje ?? "Error desconocido al cerrar sesión.";
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

                // Limpiar la sesión
                HttpContext.Session.Clear();

                if (!TempData.ContainsKey("Exito"))
                {
                    TempData["Exito"] = "Sesión cerrada correctamente. Vuelva a iniciar sesión cuando lo desee.";
                }

                return RedirectToAction(nameof(Login));
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null
                    ? $"Error interno: {ex.InnerException.Message} Contacte a soporte."
                    : $"Se produjo un error: {ex.Message} Contacte a soporte.";
            }

            return RedirectToAction(nameof(Login));
        }
    }
}
