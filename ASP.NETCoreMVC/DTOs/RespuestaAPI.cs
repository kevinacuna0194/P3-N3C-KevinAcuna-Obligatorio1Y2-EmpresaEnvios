namespace DTOs
{
    public class RespuestaAPI<T>
    {
        public string Mensaje { get; set; }
        public int Codigo { get; set; }
        public DateTime Fecha { get; set; }
        public string? Token { get; set; }
        public T Datos { get; set; }
    }

    public class EnviosDTO
    {
        public IEnumerable<EnvioDTO> EnviosComunes { get; set; }
        public IEnumerable<EnvioDTO> EnviosUrgentes { get; set; }
    }
}
