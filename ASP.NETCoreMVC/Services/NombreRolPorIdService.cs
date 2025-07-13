using DTOs;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Services
{
    public class NombreRolPorIdService
    {
        private readonly HttpClientService _httpClientService;
        private readonly ApiService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NombreRolPorIdService(HttpClientService httpClientService, ApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientService = httpClientService;
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<string> NombreRolAsync(int rolId)
        {
            string url = _apiService.GetEndpointUrl("Rol", $"Nombre/{rolId}");

            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url), "La URL del servicio de Rol no está configurada correctamente.");

            string token = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var respuesta = await _httpClientService.EnviarSolicitudAsync(url, HttpMethod.Get, token);

            if (!respuesta.IsSuccessStatusCode)
            {
                string mensaje = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine($"[Error API] Código: {respuesta.StatusCode}, Mensaje: {mensaje}");
                throw new HttpRequestException($"Error al obtener el nombre del rol con ID {rolId}.");
            }

            var body = await _httpClientService.ObtenerBodyAsync(respuesta);

            if (string.IsNullOrEmpty(body))
                throw new ArgumentNullException(nameof(body), "No se pudo obtener la respuesta del servidor.");

            var resultado = JsonConvert.DeserializeObject<RespuestaAPI<RolDTO>>(body);

            if (resultado?.Datos == null)
                throw new DatosInvalidosException("No se pudo obtener el nombre del rol. Verifica los datos e inténtalo de nuevo.");

            return resultado.Datos.Nombre;
        }
    }
}
