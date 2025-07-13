using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExcepcionesPropias
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
