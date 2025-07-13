namespace Exceptions
{
    public class DatosInvalidosException : Exception
    {
        public DatosInvalidosException() : base("Datos Inválidos")
        {
        }

        public DatosInvalidosException(string? message) : base(message)
        {
        }

        public DatosInvalidosException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
