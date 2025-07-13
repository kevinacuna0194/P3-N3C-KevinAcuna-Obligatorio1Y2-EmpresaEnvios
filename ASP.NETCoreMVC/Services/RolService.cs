using DTOs;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Services
{
    public class RolService
    {
        private readonly HttpClientService HttpClientService;
        private readonly ApiService ApiService;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public RolService(HttpClientService httpClientService, ApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            HttpClientService = httpClientService;
            ApiService = apiService;
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        }

        [HttpGet("api/rol/listado")]
        public async Task<IEnumerable<RolDTO>> ObtenerRolesAsync()
        {
            string url = ApiService.GetEndpointUrl("Rol", "Listado");

            if (string.IsNullOrEmpty(url))
            {
                throw new DatosInvalidosException("La URL del servicio de Rol no está configurada correctamente.");
            }

            string token = HttpContextAccessor.HttpContext.Session.GetString("Token");

            var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Get, token);

            if (!respuesta.IsSuccessStatusCode)
            {
                Console.WriteLine($"[Error API] Código: {respuesta.StatusCode}, Mensaje: {await respuesta.Content.ReadAsStringAsync()}");
                throw new HttpRequestException("Error al obtener la lista de roles.");
            }

            var body = await HttpClientService.ObtenerBodyAsync(respuesta);

            if (string.IsNullOrEmpty(body))
            {
                throw new DatosInvalidosException("No se encontraron roles disponibles.");
            }

            var resultado = JsonConvert.DeserializeObject<RespuestaAPI<List<RolDTO>>>(body);

            if (resultado == null || resultado.Datos == null || !resultado.Datos.Any())
            {
                throw new DatosInvalidosException("No se encontraron roles disponibles.");
            }

            return resultado.Datos;
        }

    }
}
