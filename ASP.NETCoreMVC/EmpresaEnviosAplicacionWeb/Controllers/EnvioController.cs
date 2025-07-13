using DTOs;
using EmpresaEnviosAplicacionWeb.Models;
using Enum;
using Exceptions;
using Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Presentacion.Controllers
{
    public class EnvioController : Controller
    {
        public ApiService ApiService { get; set; }
        public HttpClientService HttpClientService { get; set; }
        public AgenciaService AgenciaService { get; set; }

        public EnvioController(ApiService apiService, HttpClientService httpClientService, AgenciaService agenciaService)
        {
            ApiService = apiService;
            HttpClientService = httpClientService;
            AgenciaService = agenciaService;
        }

        // GET: EnvioController
        [ValidarSesion]
        public async Task<ActionResult> Listado()
        {
            try
            {
                // Utilizar el servicio para obtener la URL del endpoint de Envío
                string url = ApiService.GetEndpointUrl("Envio", "Listado");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente");
                }

                // Verificar si el token de sesión es válido
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud GET al servicio de Envío
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
                    var mensaje = error?.Mensaje ?? "Error desconocido al obtener listado de envios";
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

                // Deserializar la respuesta en un objeto RespuestaAPI<EnviosDTO>
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnviosDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API.");

                if (resultado.Datos is null)
                {
                    throw new DatosInvalidosException("No se encontraron envíos. Asegúrate de que existan envíos registrados");
                }

                // Crear el ViewModel para la vista y asignar los envíos comunes y urgentes
                EnviosVM enviosVM = new EnviosVM
                {
                    EnviosComunes = resultado.Datos.EnviosComunes,
                    EnviosUrgentes = resultado.Datos.EnviosUrgentes
                };

                // Convertir los pesos de los envíos de gramos a kilogramos
                enviosVM.ConvertirPesoAKilogramos();

                return View(enviosVM);
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

            // Si ocurre un error, retornar una vista vacía o con un mensaje de error
            EnviosVM enviosVM1 = new EnviosVM
            {
                EnviosComunes = new List<EnvioDTO>(),
                EnviosUrgentes = new List<EnvioDTO>()
            };

            return View();
        }

        // GET: EnvioController/DetallesEnvioComun/5
        [ValidarSesion]
        public async Task<ActionResult> DetallesEnvioComun(int id)
        {
            try
            {
                // Obtener la URL del endpoint de detalles de envío común
                string url = ApiService.GetEndpointUrl("Envio", $"DetallesEnvioComun/{id}");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente");
                }

                // Obtener el token de sesión
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud GET al servicio de Envío
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
                    var mensaje = error?.Mensaje ?? "Error desconocido al obtener detalles del envío común";
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

                // Deserializar la respuesta en un objeto EnvioDTO
                if (string.IsNullOrEmpty(body))
                {
                    throw new DatosInvalidosException("No se encontraron detalles para el envío solicitado. Asegúrate de que el ID del envío sea correcto");
                }

                // Deserialize la respuesta en un objeto RespuestaAPI<EnvioDTO>
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnvioDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API");

                // Extraer el objeto EnvioDTO de la respuesta
                EnvioDTO envioDTO = resultado.Datos;

                if (envioDTO is null)
                {
                    throw new DatosInvalidosException("No se ha encontrado un envío con el identificador proporcionado. Asegúrate de que los datos ingresados sean correctos");
                }

                // Convertir peso de gramos a kilogramos
                envioDTO.ConvertirPesoAKilogramos();

                return View(envioDTO);
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

        // GET: EnvioController/DetallesEnvioUrgente/5
        [ValidarSesion]
        public async Task<ActionResult> DetallesEnvioUrgente(int id)
        {
            try
            {
                // Obtener la URL del endpoint de detalles de envío urgente
                string url = ApiService.GetEndpointUrl("Envio", $"DetallesEnvioUrgente/{id}");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente");
                }

                // Obtener el token de sesión
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud GET al servicio de Envío Urgente
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
                    var mensaje = error?.Mensaje ?? "Error desconocido al obtener detalles del envío urgente";
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
                    throw new DatosInvalidosException("No se encontraron detalles para el envío solicitado. Asegúrate de que el ID del envío sea correcto");
                }

                // Deserializar la respuesta en un objeto RespuestaAPI<EnvioDTO>
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnvioDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API.");

                // Extraer el objeto EnvioDTO de la respuesta
                EnvioDTO envioDTO = resultado.Datos;

                if (envioDTO is null)
                {
                    throw new DatosInvalidosException("No se ha encontrado un envío con el identificador proporcionado. Asegúrate de que los datos ingresados sean correctos");
                }

                // Convertir peso de gramos a kilogramos
                envioDTO.ConvertirPesoAKilogramos();

                return View(envioDTO);
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null
                    ? $"Error interno: {ex.InnerException.Message} Contacte a soporte." :
                    $"Se produjo un error: {ex.Message} Contacte a soporte.";
            }

            return View();
        }

        // GET: EnvioController/CrearEnvioComun
        [ValidarSesion]
        public async Task<ActionResult> CrearEnvioComun()
        {
            try
            {
                var agencias = await AgenciaService.ObtenerAgenciasAsync();

                // Crear modelo para la vista
                CrearEnvioComunVM crearEnvioComunVM = new CrearEnvioComunVM
                {
                    EnvioDTO = new EnvioDTO(),
                    Agencias = agencias
                };

                return View(crearEnvioComunVM);
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

        // POST: EnvioController/CrearEnvioComun
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearEnvioComun(CrearEnvioComunVM crearEnvioComunVM)
        {
            try
            {
                string url = ApiService.GetEndpointUrl("Envio", "CrearEnvioComun");

                if (string.IsNullOrEmpty(url)) throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente.");

                // Verificar si el token de sesión es válido
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("Id");

                if (!idUsuarioLogueado.HasValue)
                {
                    throw new DatosInvalidosException("No se encontró un usuario logueado. Por favor, inicie sesión para continuar.");
                }

                // Preparar objeto DTO para la API
                CrearEnvioComunDTO crearEnvioComunDTO = new()
                {
                    UsuarioId = idUsuarioLogueado.Value,
                    EmailCliente = crearEnvioComunVM.EnvioDTO.EmailCliente,
                    Peso = crearEnvioComunVM.EnvioDTO.Peso,
                    AgenciaId = crearEnvioComunVM.EnvioDTO.AgenciaId.Value,
                };

                // Obtener el token de sesión
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud POST al servicio de Envío
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, crearEnvioComunDTO, token);

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
                    var mensaje = error?.Mensaje ?? "Error desconocido al crear el envío común";
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

                if (string.IsNullOrEmpty(body)) throw new DatosInvalidosException("No se pudo procesar la solicitud de envío.");

                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnvioDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API.");

                // Extraer el objeto EnvioDTO de la respuesta
                crearEnvioComunVM.EnvioDTO = resultado.Datos;

                TempData["Exito"] = $"Envío registrado exitosamente. <mark class='bg-success text-white rounded-pill px-2 py-1'>Número de tracking: {crearEnvioComunVM.EnvioDTO.NumeroTracking}</mark>";

                return RedirectToAction(nameof(Listado));
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null
                    ? $"Error interno: {ex.InnerException.Message} Contacte a soporte." :
                    $"Se produjo un error: {ex.Message} Contacte a soporte.";
            }

            // Si hay un error, volver a cargar la lista de agencias
            var agencias = await AgenciaService.ObtenerAgenciasAsync();

            crearEnvioComunVM.Agencias = agencias;

            // Convertir peso de gramos a kilogramos para mostrar en la vista
            crearEnvioComunVM.EnvioDTO.ConvertirPesoAKilogramos();

            return View(crearEnvioComunVM);
        }

        // GET: EnvioController/CrearEnvioUrgente
        [ValidarSesion]
        public ActionResult CrearEnvioUrgente()
        {
            try
            {
                EnvioDTO envioDTO = new EnvioDTO();

                return View(envioDTO);
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null
                    ? $"Error interno: {ex.InnerException.Message} Contacte a soporte." :
                    $"Se produjo un error: {ex.Message} Contacte a soporte.";
            }

            return View();
        }

        // POST: EnvioController/CrearEnvioUrgente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearEnvioUrgente(EnvioDTO envioDTO)
        {
            try
            {
                // Utilizar el servicio para obtener la URL del endpoint de creación de envío urgente
                string url = ApiService.GetEndpointUrl("Envio", "CrearEnvioUrgente");

                if (string.IsNullOrEmpty(url)) throw new DatosInvalidosException("La URL del servicio de Envío Urgente no está configurada correctamente.");

                // Obtener el ID del usuario autenticado
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("Id");

                if (!idUsuarioLogueado.HasValue)
                {
                    throw new DatosInvalidosException("No se encontró un usuario logueado. Por favor, inicie sesión para continuar.");
                }

                // Mapear `EnvioDTO` a `CrearEnvioUrgenteDTO` antes de enviarlo a la API
                CrearEnvioUrgenteDTO crearEnvioUrgenteDTO = new()
                {
                    UsuarioId = idUsuarioLogueado.Value,
                    EmailCliente = envioDTO.EmailCliente,
                    Peso = envioDTO.Peso,
                    DireccionPostal = envioDTO.DireccionPostal,
                };

                // Obtener el token de sesión
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud POST al servicio de Envío Urgente
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, crearEnvioUrgenteDTO, token);

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
                    var mensaje = error?.Mensaje ?? "Error desconocido al crear el envío urgente";
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
                if (string.IsNullOrEmpty(body)) throw new DatosInvalidosException("No se pudo procesar la solicitud de envío urgente.");

                // Deserializar respuesta de la API
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnvioDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API.");

                // Extraer el objeto EnvioDTO de la respuesta
                envioDTO = resultado.Datos;

                TempData["Exito"] = $"Envío urgente registrado exitosamente. <mark class='bg-success text-white rounded-pill px-2 py-1'>Número de tracking: {envioDTO.NumeroTracking}</mark>";

                return RedirectToAction(nameof(Listado));
            }
            catch (DatosInvalidosException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException != null
                    ? $"Error interno: {ex.InnerException.Message} Contacte a soporte." :
                    $"Se produjo un error: {ex.Message} Contacte a soporte.";
            }

            return View(envioDTO);
        }

        // GET: EnvioController/ListadoProcesoEntrega
        [ValidarSesion]
        public async Task<ActionResult> ListadoProcesoEntrega()
        {
            try
            {
                // Obtener la URL del endpoint de envíos en proceso de entrega
                string url = ApiService.GetEndpointUrl("Envio", "ListadoProcesoEntrega");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente");
                }

                // Verificar si el token de sesión es válido
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud GET al servicio de Envío
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
                    var mensaje = error?.Mensaje ?? "Error desconocido al obtener listado de envíos en proceso de entrega";
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
                    throw new DatosInvalidosException("No se encontraron envíos en proceso de entrega. Asegúrate de que existan envíos registrados en este estado.");
                }

                // Deserializar la respuesta en un objeto RespuestaAPI<EnviosDTO>
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnviosDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API.");

                if (resultado.Datos is null) throw new DatosInvalidosException("No se encontraron envíos en proceso de entrega. Asegúrate de que existan envíos registrados en este estado.");

                EnviosVM enviosVM = new EnviosVM
                {
                    EnviosComunes = resultado.Datos.EnviosComunes,
                    EnviosUrgentes = resultado.Datos.EnviosUrgentes
                };

                enviosVM.ConvertirPesoAKilogramos();

                return View(enviosVM);
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

            EnviosVM enviosVM1 = new EnviosVM
            {
                EnviosComunes = new List<EnvioDTO>(),
                EnviosUrgentes = new List<EnvioDTO>()
            };

            return View(enviosVM1);
        }

        // POST: EnvioController/Finalizar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Finalizar(int id, TipoEnvio tipoEnvio)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new DatosInvalidosException("Los datos del envío no son válidos. Por favor, verifica los campos e intenta nuevamente.");
                }

                // Obtener el ID del usuario autenticado
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("Id");

                if (!idUsuarioLogueado.HasValue)
                {
                    throw new DatosInvalidosException("No se encontró un usuario logueado. Por favor, inicie sesión para continuar");
                }

                // Construir el objeto DTO para la solicitud
                FinalizarDTO finalizarDTO = new FinalizarDTO
                {
                    UsuarioId = idUsuarioLogueado.Value,
                    TipoEnvio = tipoEnvio
                };

                // Obtener la URL dinámica del endpoint
                string url = ApiService.GetEndpointUrl("Envio", $"Finalizar/{id}");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente");
                }

                // Verificar si el token de sesión es válido
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud POST al servicio de Envío para finalizar el envío
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, finalizarDTO, token);

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
                    var mensaje = error?.Mensaje ?? "Error desconocido al finalizar el envío";
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
                    throw new DatosInvalidosException("No se pudo procesar la solicitud de finalización del envío. Asegúrate de que el ID del envío sea correcto.");
                }

                // Deserializar la respuesta en un objeto RespuestaAPI<EnvioDTO>
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnvioDTO>>(body);

                if (resultado is null || resultado.Datos is null)
                {
                    throw new DatosInvalidosException("Error al interpretar la respuesta de la API. Asegúrate de que el envío exista y sea válido.");
                }

                TempData["Exito"] = $"El envío con <mark class='bg-warning text-dark rounded-pill px-2 py-1'>ID: {id}</mark> ha sido finalizado con éxito. <mark class='bg-success text-white rounded-pill px-2 py-1'>Número de Tracking: {resultado.Datos.NumeroTracking}</mark>";

                return RedirectToAction(nameof(Listado));
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

            return RedirectToAction(nameof(ListadoProcesoEntrega));
        }

        // GET EnvioController/ActualizarEstado/
        [ValidarSesion]
        public async Task<ActionResult> ActualizarEstado(int id, TipoEnvio tipoEnvio)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new DatosInvalidosException("Los datos del envío no son válidos. Por favor, verifica los campos e intenta nuevamente.");
                }

                ActualizarEstadoVM actualizarEnvioVM = new ActualizarEstadoVM()
                {
                    Id = id,
                    TipoEnvio = tipoEnvio,
                    EstadosEnvio = EstadoEnvioHelper.ObtenerEstadosEnvio()
                };

                return View(actualizarEnvioVM);
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

            return RedirectToAction(nameof(ListadoProcesoEntrega));
        }

        // POST : EnvioController/ActualizarEstado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActualizarEstado(ActualizarEstadoVM actualizarEstadoVM)
        {
            try
            {
                // Validar si el estado es válido dentro del enum
                if (!System.Enum.IsDefined(typeof(EstadoEnvio), actualizarEstadoVM.EstadoSeleccionado))
                {
                    throw new DatosInvalidosException($"El estado seleccionado ('{actualizarEstadoVM.EstadoSeleccionado}') no es válido. Por favor, elige un estado correcto.");
                }

                // Obtener el ID del usuario autenticado
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("Id");

                if (!idUsuarioLogueado.HasValue)
                {
                    throw new DatosInvalidosException("No se encontró un usuario logueado. Por favor, inicie sesión para continuar.");
                }

                // Construir el DTO para la API
                ActualizarEstadoDTO actualizarEstadoDTO = new ActualizarEstadoDTO
                {
                    UsuarioId = idUsuarioLogueado.Value,
                    EstadoSeleccionado = (EstadoEnvio)actualizarEstadoVM.EstadoSeleccionado,
                    Comentario = actualizarEstadoVM.Comentario
                };

                // Obtener la URL del endpoint de actualización
                string url = ApiService.GetEndpointUrl("Envio", $"ActualizarEstado/{actualizarEstadoVM.Id}/{actualizarEstadoVM.TipoEnvio}");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente.");
                }

                // Obtener el token de sesión
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud PUT al servicio de Envío para actualizar el estado
                var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, actualizarEstadoDTO, token);

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
                    var mensaje = error?.Mensaje ?? "Error desconocido al actualizar el estado del envío";
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
                    throw new DatosInvalidosException("No se pudo procesar la solicitud de actualización del estado del envío. Asegúrate de que el ID del envío sea correcto.");
                }

                // Deserializar la respuesta en un objeto ActualizarEstadoDTO
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnvioDTO>>(body);

                // Verificar si el resultado es nulo o no contiene datos
                if (resultado is null || resultado.Datos is null)
                {
                    throw new DatosInvalidosException("No se pudo interpretar la respuesta de la API. Asegúrate de que el envío exista y sea válido.");
                }

                // Extraer el objeto EnvioDTO de la respuesta
                EnvioDTO envioActualizado = resultado.Datos;

                if (resultado is null)
                {
                    throw new DatosInvalidosException("Error al interpretar la respuesta de la API. Asegúrate de que el envío exista y sea válido.");
                }

                string estadoLegible = EstadoEnvioHelper.ObtenerDescripcionEstado(envioActualizado.Estado);

                TempData["Exito"] = $@"
                    El estado del envío con  
                    <mark class='bg-warning text-dark rounded-pill px-2 py-1'>ID: {envioActualizado.Id}</mark>  
                    ha sido actualizado a  
                    <mark class='bg-success text-white rounded-pill px-2 py-1'>{estadoLegible}</mark>.";

                return RedirectToAction(nameof(ListadoProcesoEntrega));
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

            actualizarEstadoVM.EstadosEnvio = EstadoEnvioHelper.ObtenerEstadosEnvio();

            return View(actualizarEstadoVM);
        }



        //[ValidarSesion]
        public ActionResult BuscarTracking()
        {
            BuscarTrackingVM buscarTrackingVM = new BuscarTrackingVM
            {
                EnvioDTO = new EnvioDTO(),
                TipoEnvio = null
            };

            return View(buscarTrackingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BuscarTracking(BuscarTrackingVM buscarTrackingVM)
        {
            try
            {
                if (buscarTrackingVM is null || string.IsNullOrEmpty(buscarTrackingVM.EnvioDTO.NumeroTracking))
                {
                    throw new DatosInvalidosException("El número de tracking no puede estar vacío. Por favor, ingrese un número de tracking válido.");
                }

                // Construir el DTO para la API
                BuscarTrackingDTO buscarTrackingDTO = new()
                {
                    NumeroTracking = buscarTrackingVM.EnvioDTO.NumeroTracking
                };

                // Obtener la URL del endpoint en la API
                string url = ApiService.GetEndpointUrl("Envio", "BuscarTracking");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente.");
                }

                // Obtener el ID del usuario autenticado
                var token = HttpContext.Session.GetString("Token");

                HttpResponseMessage respuesta = null;

                if (string.IsNullOrEmpty(token))
                {
                    // Enviar solicitud POST al servicio de Envío para buscar el tracking
                    respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, buscarTrackingDTO);
                }
                else
                {
                    // Enviar solicitud POST al servicio de Envío para buscar el tracking
                    respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Post, buscarTrackingDTO, token);
                }

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
                    var mensaje = error?.Mensaje ?? "Error desconocido al buscar envio por número de tracking";
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

                // Obtener el envío desde la API
                var body = await HttpClientService.ObtenerBodyAsync(respuesta);

                if (string.IsNullOrEmpty(body))
                {
                    throw new DatosInvalidosException("No se pudo procesar la solicitud de búsqueda de tracking. Asegúrate de que el número de tracking sea correcto.");
                }

                // Deserializar la respuesta en un objeto RespuestaAPI<EnvioDTO>
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnvioDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API");

                if (resultado is null || resultado.Datos is null)
                {
                    throw new DatosInvalidosException("No se encontraron datos para el número de tracking proporcionado. Asegúrate de que el número de tracking sea correcto.");
                }

                // Asignar el envío encontrado al DTO de la vista
                buscarTrackingVM.EnvioDTO = resultado.Datos;

                // Definir el tipo de envío según la lógica
                buscarTrackingVM.TipoEnvio = buscarTrackingVM.EnvioDTO.AgenciaId > 0
                    ? TipoEnvio.Comun
                    : TipoEnvio.Urgente;

                // Convertir el peso de gramos a kilogramos para mostrar en la vista
                buscarTrackingVM.ConvertirPesoAKilogramos();

                buscarTrackingVM.BusquedaEjecutada = true;

                return View(buscarTrackingVM);
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

            return View(buscarTrackingVM);
        }

        // GET: EnvioController/ListadoCliente
        [ValidarSesion]
        public async Task<ActionResult> ListadoCliente()
        {
            try
            {
                int idUsuarioLogueado = HttpContext.Session.GetInt32("Id") ?? throw new DatosInvalidosException("No se encontró un usuario logueado. Por favor, inicie sesión para continuar.");

                // Utilizar el servicio para obtener la URL del endpoint de listado de envíos del cliente
                string url = ApiService.GetEndpointUrl("Envio", $"ListadoCliente/{idUsuarioLogueado}");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente");
                }

                // Verificar si el token de sesión es válido
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud GET al servicio de Envío
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
                    var mensaje = error?.Mensaje ?? "Error desconocido al obtener listado de envíos del cliente";
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
                    throw new DatosInvalidosException("No se encontraron envíos del cliente. Asegúrate de que existan envíos registrados para el usuario actual.");
                }

                // Deserializar la respuesta en un objeto RespuestaAPI<EnviosDTO>
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnviosDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API.");

                if (resultado.Datos is null) throw new DatosInvalidosException("No se encontraron envíos del cliente. Asegúrate de que existan envíos registrados para el usuario actual.");

                EnviosVM enviosVM = new EnviosVM
                {
                    EnviosComunes = resultado.Datos.EnviosComunes,
                    EnviosUrgentes = resultado.Datos.EnviosUrgentes
                };

                enviosVM.ConvertirPesoAKilogramos();

                return View(enviosVM);
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

            EnviosVM enviosVM1 = new EnviosVM
            {
                EnviosComunes = new List<EnvioDTO>(),
                EnviosUrgentes = new List<EnvioDTO>()
            };

            enviosVM1.ConvertirPesoAKilogramos();

            return View(enviosVM1);
        }

        // GET: EnvioController/FormBuscarPorEstadoYRangoFechas
        [ValidarSesion]
        public ActionResult FormBuscarPorEstadoYRangoFechas()
        {
            try
            {
                string? rol = HttpContext.Session.GetString("Rol");

                if (rol != "Cliente")
                {
                    throw new DatosInvalidosException("Acceso denegado. Esta funcionalidad solo está disponible para clientes.");
                }

                FormBuscarPorEstadoYRangoFechasVM buscarPorEstadoYRangoFechasVM = new FormBuscarPorEstadoYRangoFechasVM
                {
                    FechaInicio = DateTime.Now.AddDays(-30), // Por defecto, 30 días atrás
                    FechaFin = DateTime.Now, // Por defecto, hoy
                    EstadosEnvio = EstadoEnvioHelper.ObtenerEstadosEnvio(),
                    EstadoSeleccionado = (int)EstadoEnvio.Todos // Por defecto, todos los estados seleccionados
                };

                return View(buscarPorEstadoYRangoFechasVM);
            }
            catch (DatosInvalidosException ex)
            {
                return View("VistaExcepcion", ex.Message);
            }
            catch (Exception ex)
            {
                var mensaje = ex.InnerException != null
                ? $"Error interno: {ex.InnerException.Message} Contacte a soporte."
                : $"Se produjo un error: {ex.Message} Contacte a soporte.";

                return View("VistaExcepcion", mensaje);
            }
        }

        // GET: EnviosController/BuscarPorEstadoYRangoFechas/{estadoEnvio}/{fechaInicio}/{fechaFin}
        [ValidarSesion]
        public async Task<ActionResult> BuscarPorEstadoYRangoFechas(int EstadoSeleccionado, DateTime fechaInicio, DateTime fechaFin, FormBuscarPorEstadoYRangoFechasVM formBuscarPorEstadoYRangoFechasVM)
        {
            try
            {
                string? rol = HttpContext.Session.GetString("Rol");

                if (rol != "Cliente")
                {
                    throw new DatosInvalidosException("Acceso denegado. Solo los clientes pueden buscar envíos por comentario.");
                }

                if (!System.Enum.IsDefined(typeof(EstadoEnvio), EstadoSeleccionado))
                {
                    throw new DatosInvalidosException($"El estado '{EstadoSeleccionado}' no es válido.");
                }

                EstadoEnvio estadoEnum = (EstadoEnvio)EstadoSeleccionado;

                // Validar el rango de fechas
                if (fechaInicio > fechaFin)
                {
                    throw new DatosInvalidosException("La fecha de inicio no puede ser posterior a la fecha de fin. Por favor, verifica las fechas e intenta nuevamente.");
                }

                // Obtener el ID del usuario autenticado
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("Id");

                if (!idUsuarioLogueado.HasValue)
                {
                    throw new DatosInvalidosException("No se encontró un usuario logueado. Por favor, inicie sesión para continuar.");
                }

                var fechaInicioStr = Uri.EscapeDataString(fechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"));
                var fechaFinStr = Uri.EscapeDataString(fechaFin.ToString("yyyy-MM-ddTHH:mm:ss"));

                string url = ApiService.GetEndpointUrl(
                    "Envio",
                    $"BuscarPorEstadoYRangoFechas/{estadoEnum}/{fechaInicioStr}/{fechaFinStr}?idCliente={idUsuarioLogueado.Value}"
                );

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente");
                }

                // Verificar si el token de sesión es válido
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud POST al servicio de Envío para buscar envíos por estado y rango de fechas
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
                    var mensaje = error?.Mensaje ?? "Error desconocido al buscar envíos por estado y rango de fechas";
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
                    throw new DatosInvalidosException("No se encontraron envíos en el rango de fechas y estado especificados. Asegúrate de que existan envíos registrados.");
                }

                // Deserializar la respuesta en un objeto RespuestaAPI<EnviosDTO>
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnviosDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API.");

                if (resultado.Datos is null) throw new DatosInvalidosException("No se encontraron envíos en el rango de fechas y estado especificados. Asegúrate de que existan envíos registrados.");

                EnviosVM enviosVM = new EnviosVM
                {
                    EnviosComunes = resultado.Datos.EnviosComunes,
                    EnviosUrgentes = resultado.Datos.EnviosUrgentes
                };

                formBuscarPorEstadoYRangoFechasVM.EnviosVM = enviosVM;

                formBuscarPorEstadoYRangoFechasVM.EnviosVM.ConvertirPesoAKilogramos();

                formBuscarPorEstadoYRangoFechasVM.BusquedaEjecutada = true;

                formBuscarPorEstadoYRangoFechasVM.EstadosEnvio = EstadoEnvioHelper.ObtenerEstadosEnvio();

                return View("FormBuscarPorEstadoYRangoFechas", formBuscarPorEstadoYRangoFechasVM);
            }
            catch (DatosInvalidosException ex)
            {
                return View("VistaExcepcion", ex.Message);
            }
            catch (Exception ex)
            {
                var mensaje = ex.InnerException != null
                ? $"Error interno: {ex.InnerException.Message} Contacte a soporte."
                : $"Se produjo un error: {ex.Message} Contacte a soporte.";

                return View("VistaExcepcion", mensaje);
            }
        }

        // GET: EnvioController/FormBuscarEnviosPorComentario
        [ValidarSesion]
        public ActionResult FormBuscarPorComentario()
        {
            try
            {
                string? rol = HttpContext.Session.GetString("Rol");

                if (rol != "Cliente")
                {
                    throw new DatosInvalidosException("Acceso denegado. Solo los clientes pueden buscar envíos por comentario.");
                }

                return View(new FormBuscarPorComentarioVM());
            }
            catch (DatosInvalidosException ex)
            {
                return View("VistaExcepcion", ex.Message);
            }
            catch (Exception ex)
            {
                var mensaje = ex.InnerException != null
                ? $"Error interno: {ex.InnerException.Message} Contacte a soporte."
                : $"Se produjo un error: {ex.Message} Contacte a soporte.";

                return View("VistaExcepcion", mensaje);
            }
        }

        // GET: EnvioController/BuscarEnviosPorComentario
        [ValidarSesion]
        public async Task<ActionResult> BuscarPorComentario(FormBuscarPorComentarioVM formBuscarPorComentarioVM)
        {
            try
            {
                string? rol = HttpContext.Session.GetString("Rol");

                if (rol != "Cliente")
                {
                    throw new DatosInvalidosException("Acceso denegado. Solo los clientes pueden buscar envíos por comentario.");
                }

                if (formBuscarPorComentarioVM == null)
                {
                    throw new ArgumentNullException(nameof(formBuscarPorComentarioVM));
                }

                // Obtener el ID del usuario autenticado
                int? idUsuarioLogueado = HttpContext.Session.GetInt32("Id");

                if (!idUsuarioLogueado.HasValue)
                {
                    throw new DatosInvalidosException("No se encontró un usuario logueado. Por favor, inicie sesión para continuar.");
                }

                string url = ApiService.GetEndpointUrl("Envio", $"BuscarPorComentario/{formBuscarPorComentarioVM.Comentario.Trim()}?idCliente={idUsuarioLogueado}");

                if (string.IsNullOrEmpty(url))
                {
                    throw new DatosInvalidosException("La URL del servicio de Envío no está configurada correctamente");
                }

                // Verificar si el token de sesión es válido
                var token = HttpContext.Session.GetString("Token");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["Error"] = "Token de sesión no válido o expirado.";
                    return RedirectToAction("Login", "Home");
                }

                // Enviar solicitud POST al servicio de Envío para buscar envíos por estado y rango de fechas
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
                    var mensaje = error?.Mensaje ?? "Error desconocido al buscar envíos por estado y rango de fechas";
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
                    throw new DatosInvalidosException("No se encontraron envíos en el rango de fechas y estado especificados. Asegúrate de que existan envíos registrados.");
                }

                // Deserializar la respuesta en un objeto RespuestaAPI<EnviosDTO>
                var resultado = JsonConvert.DeserializeObject<RespuestaAPI<EnviosDTO>>(body) ?? throw new DatosInvalidosException("Error al interpretar la respuesta de la API.");

                if (resultado.Datos is null) throw new DatosInvalidosException("No se encontraron envíos en el rango de fechas y estado especificados. Asegúrate de que existan envíos registrados.");

                EnviosVM enviosVM = new EnviosVM
                {
                    EnviosComunes = resultado.Datos.EnviosComunes,
                    EnviosUrgentes = resultado.Datos.EnviosUrgentes
                };

                enviosVM.ConvertirPesoAKilogramos();

                formBuscarPorComentarioVM.EnviosVM = enviosVM;

                formBuscarPorComentarioVM.BusquedaEjecutada = true;

                return View("FormBuscarPorComentario", formBuscarPorComentarioVM);

            }
            catch (DatosInvalidosException ex)
            {
                return View("VistaExcepcion", ex.Message);
            }
            catch (Exception ex)
            {
                var mensaje = ex.InnerException != null
                ? $"Error interno: {ex.InnerException.Message} Contacte a soporte."
                : $"Se produjo un error: {ex.Message} Contacte a soporte.";

                return View("VistaExcepcion", mensaje);
            }
        }

        // GET: EnvioController/Editar/5
        [ValidarSesion]
        public ActionResult Editar(int id)
        {
            TempData["Error"] = $"La acción de edición aún no está implementada. No es posible editar el envío con ID {id} en este momento.";

            Console.WriteLine($"[Info] Intento de acceder a la acción no implementada: Editar envío con ID {id}.");

            return RedirectToAction(nameof(Listado));
        }

        // POST: EnvioController/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, IFormCollection collection)
        {
            TempData["Error"] = $"La acción de edición aún no está implementada. No es posible editar el envío con ID {id} en este momento.";

            Console.WriteLine($"[Info] Intento de acceder a la acción no implementada: Editar envío con ID {id}.");

            return RedirectToAction(nameof(Listado));
        }

        // GET: EnvioController/Eliminar/5
        [ValidarSesion]
        public ActionResult Eliminar(int id)
        {
            TempData["Error"] = $"La acción de eliminación aún no está implementada. No es posible eliminar el envío con ID {id} en este momento.";

            Console.WriteLine($"[Info] Intento de acceder a la acción no implementada: Eliminar envío con ID {id}.");

            return RedirectToAction(nameof(Listado));
        }


        // POST: EnvioController/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, IFormCollection collection)
        {
            TempData["Error"] = $"La acción de eliminación aún no está implementada. No es posible eliminar el envío con ID {id} en este momento.";

            Console.WriteLine($"[Info] Intento de acceder a la acción no implementada: Eliminar envío con ID {id}.");

            return RedirectToAction(nameof(Listado));
        }
    }
}
