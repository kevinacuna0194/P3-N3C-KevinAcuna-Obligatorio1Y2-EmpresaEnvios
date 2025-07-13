using ExcepcionesPropias;
using Microsoft.EntityFrameworkCore;

namespace LogicaNegocio.ValueObjects
{
    [Owned]
    public record DocumentoIdentidadUsuario
    {
        public int DocumentoIdentidad { get; init; }

        public DocumentoIdentidadUsuario(int documentoIdentidad)
        {
            DocumentoIdentidad = documentoIdentidad;
            Validar();
        }

        public DocumentoIdentidadUsuario()
        {
        }

        public void Validar()
        {
            if (ValidarCedula(DocumentoIdentidad) is false)
            {
                throw new DatosInvalidosException("El número de cédula no es válido. Debe tener 8 dígitos y cumplir con el algoritmo de validación");
            }
        }

        private bool ValidarCedula(int cedula)
        {
            // Convertir el número entero a una cadena para separar el dígito verificador
            string cedulaStr = cedula.ToString();
            // Verificar que tenga exactamente 8 caracteres
            if (cedulaStr.Length < 6 || cedulaStr.Length > 8)
            {
                return false;
            }
            // Separar el dígito verificador y los dígitos principales
            int digitoVerificador = cedula % 10; // Último dígito
            int numerosPrincipales = cedula / 10; // Los primeros siete dígitos
                                                  // Coeficientes definidos para la validación
            int[] coeficientes = { 2, 9, 8, 7, 6, 3, 4 };
            int suma = 0;
            // Calcular la suma ponderada
            for (int i = coeficientes.Length - 1; i >= 0; i--)
            {
                suma += (numerosPrincipales % 10) * coeficientes[i]; // Último dígito multiplicado por el coeficiente
                numerosPrincipales /= 10; // Eliminar el último dígito
            }
            // Calcular el módulo y obtener el dígito verificador esperado
            int modulo = suma % 10;
            int digitoCalculado = modulo == 0 ? 0 : 10 - modulo;
            // Retornar si el dígito verificador coincide con el calculado
            return digitoVerificador == digitoCalculado;
        }
    }
}
