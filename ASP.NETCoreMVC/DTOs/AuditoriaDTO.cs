namespace DTOs
{
    public class AuditoriaDTO
    {
        public int Id { get; set; }
        public string Accion { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Detalle { get; set; }
    }
}
