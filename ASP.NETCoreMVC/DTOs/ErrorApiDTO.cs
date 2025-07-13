using System.Diagnostics;

namespace DTOs
{
    public class ErrorApiDTO
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaError { get; set; }
        public string TipoError { get; set; }
        public string? CampoFaltante { get; set; }
        public string Detalles { get; set; }
        public string? StackTrace { get; set; }
        public string SolucionSugerida { get; set; }

    }
}
