using Enum;

namespace DTOs
{
    public class ActualizarEstadoDTO
    {
        public int UsuarioId { get; set; }
        public EstadoEnvio EstadoSeleccionado { get; set; }
        public string Comentario { get; set; }
    }
}
