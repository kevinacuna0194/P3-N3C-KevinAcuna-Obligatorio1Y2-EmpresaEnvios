using Configurations;
using Microsoft.Extensions.Options;

namespace Services
{
    public class ApiService
    {
        private readonly ApiSettings ApiSettings;

        public ApiService(IOptions<ApiSettings> apiSettings)
        {
            ApiSettings = apiSettings.Value;
        }

        public string GetEndpointUrl(string key, string additionalPath)
        {
            if (ApiSettings.Endpoints.ContainsKey(key))
            {
                return $"{ApiSettings.BaseUrl}/{ApiSettings.Endpoints[key]}{(string.IsNullOrEmpty(additionalPath) ? "" : "/" + additionalPath)}";
            }
            return null; // Manejo de error si la clave no existe
        }

        public string GetEndpointUrl(string key)
        {
            return GetEndpointUrl(key, "");
        }
    }
}
