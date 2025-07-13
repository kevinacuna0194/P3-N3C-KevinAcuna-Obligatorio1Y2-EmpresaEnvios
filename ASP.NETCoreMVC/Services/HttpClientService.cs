using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Services
{
    public class HttpClientService
    {
        private readonly HttpClient HttpClient;

        public HttpClientService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        // Método principal con todos los parámetros
        public async Task<HttpResponseMessage> EnviarSolicitudAsync(string url, HttpMethod metodo, object? contenido = null, string? token = null)
        {
            using var request = new HttpRequestMessage(metodo, url);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (contenido is not null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(contenido), Encoding.UTF8, "application/json");
            }

            return await HttpClient.SendAsync(request);
        }

        // Sobrecarga SIN contenido ni token (GET / DELETE)
        public async Task<HttpResponseMessage> EnviarSolicitudAsync(string url, HttpMethod metodo)
        {
            return await EnviarSolicitudAsync(url, metodo, null, null);
        }

        // Sobrecarga SOLO con token (GET / DELETE con autenticación)
        public async Task<HttpResponseMessage> EnviarSolicitudAsync(string url, HttpMethod metodo, string token)
        {
            return await EnviarSolicitudAsync(url, metodo, null, token);
        }

        // Sobrecarga SOLO con contenido (POST / PUT)
        public async Task<HttpResponseMessage> EnviarSolicitudAsync(string url, HttpMethod metodo, object contenido)
        {
            return await EnviarSolicitudAsync(url, metodo, contenido, null);
        }

        // Método para obtener el contenido de la respuesta
        public async Task<string> ObtenerBodyAsync(HttpResponseMessage respuesta)
        {
            return await respuesta.Content.ReadAsStringAsync();
        }
    }
}
