using DTOs;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Services
{
    public class AgenciaService
    {
        private readonly HttpClientService HttpClientService;
        private readonly ApiService ApiService;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public AgenciaService(HttpClientService httpClientService, ApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            HttpClientService = httpClientService;
            ApiService = apiService;
            HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        }

        public async Task<IEnumerable<AgenciaDTO>> ObtenerAgenciasAsync()
        {
            string url = ApiService.GetEndpointUrl("Agencia", "Listado");

            if (string.IsNullOrEmpty(url))
            {
                throw new DatosInvalidosException("La URL del servicio de Agencia no está configurada correctamente.");
            }

            string token = HttpContextAccessor.HttpContext.Session.GetString("Token");

            var respuesta = await HttpClientService.EnviarSolicitudAsync(url, HttpMethod.Get, token);

            if (!respuesta.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error al obtener agencias de la API.");
            }

            var body = await HttpClientService.ObtenerBodyAsync(respuesta);

            var resultado = JsonConvert.DeserializeObject<RespuestaAPI<List<AgenciaDTO>>>(body);

            if (resultado == null || resultado.Datos == null)
            {
                throw new DatosInvalidosException("No se encontraron agencias en la respuesta de la API");
            }

            var agencias = resultado.Datos;

            if (agencias is null || !agencias.Any())
            {
                throw new DatosInvalidosException("No se encontraron agencias en la API.");
            }

            return agencias;
        }

    }
}
