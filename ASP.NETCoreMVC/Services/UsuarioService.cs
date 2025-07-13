using DTOs;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Services
{
    public class UsuarioService
    {
        private readonly HttpClientService HttpClientService;
        private readonly ApiService ApiService;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public UsuarioService(HttpClientService httpClientService, ApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            HttpClientService = httpClientService;
            ApiService = apiService;
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        }

        public async Task<UsuarioDTO> ObtenerUsuarioAsync(int id)
        {
            string url = ApiService.GetEndpointUrl("Usuario", $"Detalles/{id}");

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url), "La URL del servicio de Usuario no está configurada correctamente.");
            }

            string token = HttpContextAccessor.HttpContext.Session.GetString("Token");

            var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Get, token);

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error al obtener los detalles del usuario con ID {id}. Código: {respuesta.StatusCode}");
            }

            var body = await HttpClientService.ObtenerBodyAsync(respuesta);

            if (string.IsNullOrEmpty(body))
            {
                throw new DatosInvalidosException($"No se encontraron detalles para el usuario con ID {id}.");
            }

            var resultado = JsonConvert.DeserializeObject<RespuestaAPI<UsuarioDTO>>(body);

            if (resultado?.Datos == null)
            {
                throw new DatosInvalidosException($"No se encontraron detalles para el usuario con ID {id}.");
            }

            return resultado.Datos;
        }
    }
}
