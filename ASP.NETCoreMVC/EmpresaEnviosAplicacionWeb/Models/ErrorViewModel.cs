namespace EmpresaEnviosAplicacionWeb.Models
{
    public class ErrorViewModel
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public string Sugerencia { get; set; }
        public string Detalles { get; set; }
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
