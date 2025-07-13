using DTOs;
using EmpresaEnviosAplicacionWeb.Models;
using Exceptions;
using Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using System.Diagnostics;

namespace EmpresaEnviosAplicacionWeb.Controllers
{
    public class AuditoriaController : Controller
    {
        public ApiService ApiService { get; set; }
        public HttpClientService HttpClientService { get; set; }

        public AuditoriaController(ApiService apiService, HttpClientService httpClientService)
        {
            ApiService = apiService;
            HttpClientService = httpClientService;
        }

        // GET: AuditoriaController/Listado
        [HttpGet]
        [ValidarSesion]
        public async Task<ActionResult> Listado()
        {
            try
            {
                // Utilizar el servicio para obtener la URL del endpoint de Auditoría
                string url = ApiService.GetEndpointUrl("Auditoria", "Listado");

                if (string.IsNullOrEmpty(url))
                {
                    throw new Exception("La URL del servicio de Auditoría no está configurada correctamente");
                }

                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud GET al servicio de Auditoría
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
                    var mensaje = error?.Mensaje ?? "Error desconocido al obtener listado de auditoría.";
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

                // Obtener el resultado de la deserialización
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<List<AuditoriaDTO>>>(body);

                if (resultado == null || resultado.Datos == null || !resultado.Datos.Any())
                {
                    TempData["Error"] = "No se encontraron auditorías registradas";
                    return View();
                }

                // Deserializar el cuerpo de la respuesta en una lista de AuditoriaDTO
                var listaAuditorias = resultado.Datos;

                return View(listaAuditorias);
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null
                    ? $"Error interno: {ex.InnerException.Message}. Contacte a soporte."
                    : $"Se produjo un error: {ex.Message}. Contacte a soporte.";
            }

            return View();
        }

        // GET: AuditoriaController/Detalles/5
        [HttpGet]
        [ValidarSesion]
        public async Task<ActionResult> Detalles(int id)
        {
            try
            {
                // Llamar al servicio para obtener los detalles de la auditoría por ID
                string url = ApiService.GetEndpointUrl("Auditoria", $"Detalles/{id}");

                if (string.IsNullOrEmpty(url))
                {
                    TempData["Error"] = "La URL del servicio de Auditoría no está configurada correctamente";
                    return RedirectToAction(nameof(Listado));
                }

                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

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
                    var mensaje = error?.Mensaje ?? "Error desconocido al obtener detalles de auditoría.";
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
                    TempData["Error"] = "No se encontraron detalles para la auditoría solicitada";
                    return RedirectToAction(nameof(Listado));
                }

                // Deserializar el cuerpo de la respuesta en un objeto AuditoriaDTO
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<AuditoriaDTO>>(body);

                if (resultado == null || resultado.Datos == null)
                {
                    TempData["Error"] = "No se encontraron detalles para la auditoría solicitada";
                    return RedirectToAction(nameof(Listado));
                }

                var auditoriaDTO = resultado.Datos;

                return View(auditoriaDTO);
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

            return View();
        }

        // GET: AuditoriaController/Crear
        [ValidarSesion]
        public ActionResult Crear()
        {
            return View();
        }

        // POST: AuditoriaController/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Listado));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuditoriaController/Editar/5
        [ValidarSesion]
        public ActionResult Editar(int id)
        {
            return View();
        }

        // POST: AuditoriaController/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Listado));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuditoriaController/Eliminar/5
        [ValidarSesion]
        public ActionResult Eliminar(int id)
        {
            return View();
        }

        // POST: AuditoriaController/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Listado));
            }
            catch
            {
                return View();
            }
        }
    }
}
