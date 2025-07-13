using ExcepcionesPropias;
using LogicaNegocio.InterfacesDominio;
using LogicaNegocio.ValueObjects;

namespace LogicaNegocio.EntidadesDominio
{
    public class Usuario : IValidable
    {
        // Propiedades
        public int Id { get; set; }
        public NombreUsuario Nombre { get; set; }
        public EmailUsuario Email { get; set; }
        public PasswordUsuario Password { get; set; }
        public FechaRegistroUsuario FechaRegistro { get; set; }
        public FechaNacimientoUsuario FechaNacimiento { get; set; }
        public TelefonoUsuario Telefono { get; set; }
        public DireccionUsuario Direccion { get; set; }
        public DocumentoIdentidadUsuario DocumentoIdentidad { get; set; }
        public Rol Rol { get; set; }

        // Constructor sin parámetros
        public Usuario() { }

        // Constructor con parámetros
        public Usuario(NombreUsuario nombre, EmailUsuario email, PasswordUsuario contrasena, FechaNacimientoUsuario fechaNacimiento, TelefonoUsuario telefono, DireccionUsuario direccion, DocumentoIdentidadUsuario documentoIdentidad, Rol rol)
        {
            Nombre = nombre;
            Email = email;
            Password = contrasena;
            FechaNacimiento = fechaNacimiento;
            Telefono = telefono;
            Direccion = direccion;
            DocumentoIdentidad = documentoIdentidad;
            Rol = rol;
        }

        // Métodos
        public void Validar()
        {
            if (Rol is null) throw new DatosInvalidosException("El Rol es obligatorio");
        }
    }
}
