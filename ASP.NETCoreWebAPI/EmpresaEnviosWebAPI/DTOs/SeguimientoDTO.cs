namespace EmpresaEnviosWebAPI.DTOs
{
    public class SeguimientoDTO
    {
        public int Id { get; set; }
        public DateTime FechaSeguimiento { get; set; }
        public string EmpleadoNombre { get; set; }
        public string Comentario { get; set; }
    }
}
