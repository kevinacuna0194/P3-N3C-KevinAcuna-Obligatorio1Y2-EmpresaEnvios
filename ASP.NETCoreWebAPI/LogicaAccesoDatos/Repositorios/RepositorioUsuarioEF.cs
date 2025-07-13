using ExcepcionesPropias;
using LogicaAccesoDatos.EntityFramework;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioUsuarioEF : IRepositorioUsuario
    {
        public LibraryContext LibraryContext { get; set; }

        public RepositorioUsuarioEF(LibraryContext libraryContext)
        {
            LibraryContext = libraryContext;
        }

        /// Método para agregar un nuevo usuario
        public void Add(Usuario user, int idUsuarioLogueado)
        {
            if (user is null)
            {
                throw new DatosInvalidosException("El usuario no puede ser nulo");
            }

            Usuario usuario = ObtenerUsuarioPorNombre(user.Nombre.Nombre);

            if (usuario is not null)
            {
                throw new DatosInvalidosException($"Ya existe un usuario con el nombre de usuario: {usuario.Nombre.Nombre}.");
            }

            Usuario usuario1 = ObtenerUsuarioPorEmail(user.Email.Email);

            if (usuario1 is not null)
            {
                throw new DatosInvalidosException($"Ya existe un usuario con el email: {usuario.Email.Email}.");
            }

            LibraryContext.Entry(user.Rol).State = EntityState.Unchanged;
            LibraryContext.Usuarios.Add(user);
            LibraryContext.SaveChanges();

            // Verificar si el usuario fue creado correctamente
            bool exist = LibraryContext.Usuarios.Any(u => u.Id == user.Id);

            if (!exist)
            {
                throw new DatosInvalidosException("No se pudo crear el usuario.");
            }

            // Obtener la fecha y hora actual en UTC
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo uruguayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Montevideo Standard Time");
            DateTime uruguayTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, uruguayTimeZone);

            // Crear la auditoría
            if (exist)
            {
                Auditoria auditoria = new Auditoria
                {
                    Accion = new AccionAuditoria { Accion = "Crear" },
                    Fecha = new FechaAuditoria { Fecha = uruguayTime },
                    Usuario = new Usuario { Id = idUsuarioLogueado },
                    Detalle = new DetalleAuditoria { Detalle = $"Se ha creado un nuevo usuario: {user.Nombre.Nombre} Id: {user.Id}" }
                };

                // Guardar la auditoría
                LibraryContext.Entry(auditoria.Usuario).State = EntityState.Unchanged;
                LibraryContext.Auditorias.Add(auditoria);
                LibraryContext.SaveChanges();
            }
        }

        /// Método para obtener todos los usuarios
        public List<Usuario> FindAll()
        {
            return LibraryContext.Usuarios
                .Include(user => user.Rol)
                .ToList();
        }

        /// Método para obtener un usuario por su ID
        public Usuario FindById(int id)
        {
            if (id <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario no puede ser menor o igual a cero");
            }

            return LibraryContext.Usuarios.Include(user => user.Rol).FirstOrDefault(user => user.Id == id);
        }

        /// Método para obtener un usuario por su email
        public Usuario ObtenerUsuarioPorEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new DatosInvalidosException("El email no puede ser nulo o vacío");
            }

            return LibraryContext.Usuarios.Include(user => user.Rol).Where(user => user.Email.Email == email).AsNoTracking().SingleOrDefault();
        }

        /// Método para obtener un usuario por su email y contraseña
        public Usuario ObtenerUsuarioPorEmailYPassword(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new DatosInvalidosException("El email y la contraseña no pueden ser nulos o vacíos");
            }

            return LibraryContext.Usuarios
                .Where(user => user.Email.Email == email && user.Password.Password == password)
                .Include(user => user.Rol)
                .FirstOrDefault();
        }

        /// Método para obtener un usuario por su documento de identidad
        public Usuario ObtenerUsuarioPorDocumentoIdentidad(int identityDocument)
        {
            if (identityDocument <= 0)
            {
                throw new DatosInvalidosException("El documento de identidad no puede ser menor o igual a cero");
            }

            return LibraryContext.Usuarios.Include(user => user.Rol).Where(user => user.DocumentoIdentidad.DocumentoIdentidad == identityDocument).FirstOrDefault();
        }

        /// Método para obtener un usuario por su nombre de usuario
        public Usuario ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                throw new DatosInvalidosException("El nombre de usuario no puede ser nulo o vacío");
            }

            return LibraryContext.Usuarios.Include(user => user.Rol).Where(user => user.Nombre.Nombre == nombreUsuario).FirstOrDefault();
        }

        /// Método para obtener una lista de usuarios por su rol
        public List<Usuario> ObtenerUsuariosPorRol(Rol rol)
        {
            if (rol is null)
            {
                throw new DatosInvalidosException("El rol no puede ser nulo");
            }

            return LibraryContext.Usuarios.Include(user => user.Rol).Where(user => user.Rol == rol).ToList();
        }

        /// Método para eliminar un usuario por su ID
        public void Remove(int id, int idUsuarioLogueado)
        {
            if (id <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario no puede ser menor o igual a cero.");
            }

            Usuario usuario = FindById(id);

            if (usuario is null)
            {
                throw new DatosInvalidosException($"No se encontró un usuario con el ID: {id}.");
            }

            if (LibraryContext.Envios.Any(e => e.Empleado.Id == usuario.Id || e.Cliente.Id == usuario.Id))
            {
                throw new DatosInvalidosException($"No se puede eliminar el usuario con ID: {id} porque está asociado a envíos.");
            }

            LibraryContext.Usuarios.Remove(usuario);
            LibraryContext.SaveChanges();

            // Verificar si el usuario fue eliminado correctamente
            bool exist = LibraryContext.Usuarios.Any(u => u.Id == usuario.Id);

            if (exist)
            {
                throw new DatosInvalidosException($"No se pudo eliminar el usuario con el ID: {id}.");
            }

            // Obtener la fecha y hora actual en UTC
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo uruguayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Montevideo Standard Time");
            DateTime uruguayTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, uruguayTimeZone);

            // Crear la auditoría
            Auditoria auditoria = new Auditoria
            {
                Accion = new AccionAuditoria { Accion = "Eliminar" },
                Fecha = new FechaAuditoria { Fecha = uruguayTime },
                Usuario = new Usuario { Id = idUsuarioLogueado },
                Detalle = new DetalleAuditoria { Detalle = $"Se eliminó el usuario: {usuario.Nombre.Nombre} Id: {usuario.Id}" }
            };

            // Guardar la auditoría
            LibraryContext.Entry(auditoria.Usuario).State = EntityState.Unchanged;
            LibraryContext.Auditorias.Add(auditoria);
            LibraryContext.SaveChanges();
        }

        /// Método para actualizar un usuario
        public void Update(Usuario entidad, int idUsuarioLogueado)
        {
            if (entidad is null)
            {
                throw new DatosInvalidosException("El usuario no puede ser nulo.");
            }

            Usuario usuario = FindById(entidad.Id);

            if (usuario is null)
            {
                throw new DatosInvalidosException($"No se encontró un usuario con el ID: {entidad.Id}.");
            }

            if (usuario.Nombre.Nombre != entidad.Nombre.Nombre)
            {
                Usuario user = ObtenerUsuarioPorNombre(entidad.Nombre.Nombre);

                if (user is not null)
                {
                    throw new DatosInvalidosException($"Ya existe un usuario con el nombre de usuario: {entidad.Nombre.Nombre}.");
                }
            }

            if (usuario.Email.Email != entidad.Email.Email)
            {
                Usuario user = ObtenerUsuarioPorEmail(entidad.Email.Email);

                if (user is not null)
                {
                    throw new DatosInvalidosException($"Ya existe el usuario con email: {entidad.Email.Email}.");
                }
            }

            LibraryContext.Entry(usuario).State = EntityState.Detached;
            LibraryContext.Entry(usuario.Rol).State = EntityState.Detached;
            LibraryContext.Update(entidad);
            LibraryContext.SaveChanges();

            // Verificar si el usuario fue actualizado correctamente
            bool exist = LibraryContext.Usuarios.Any(u => u.Id == usuario.Id);

            if (!exist)
            {
                throw new DatosInvalidosException($"No se pudo actualizar el usuario con el ID: {entidad.Id}.");
            }

            // Obtener la fecha y hora actual en UTC
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo uruguayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Montevideo Standard Time");
            DateTime uruguayTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, uruguayTimeZone);

            // Verificar si EntityFramework esta haciendo seguimiento al usuario
            var usuarioExistente = LibraryContext.Usuarios.Local.SingleOrDefault(u => u.Id == idUsuarioLogueado);

            // Si no está en el contexto local, crear una nueva instancia
            if (usuarioExistente == null)
            {
                usuarioExistente = new Usuario { Id = idUsuarioLogueado };
                LibraryContext.Attach(usuarioExistente);
            }

            // Crear la auditoría
            if (exist)
            {
                Auditoria auditoria = new Auditoria
                {
                    Accion = new AccionAuditoria { Accion = "Actualizar" },
                    Fecha = new FechaAuditoria { Fecha = uruguayTime },
                    Usuario = usuarioExistente,
                    Detalle = new DetalleAuditoria { Detalle = $"Se actualizó el usuario: {usuario.Nombre.Nombre} Id: {usuario.Id}" }
                };

                // Guardar la auditoría
                LibraryContext.Entry(auditoria.Usuario).State = EntityState.Unchanged;
                LibraryContext.Auditorias.Add(auditoria);
                LibraryContext.SaveChanges();
            }
        }

        /// Método para verificar si un usuario existe por su ID
        public bool Existe(int id)
        {
            if (id <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario no puede ser menor o igual a cero");
            }

            return LibraryContext.Usuarios.Any(user => user.Id == id);
        }

        public void Add(Usuario entidad)
        {
            throw new NotImplementedException();
        }

        public void Update(Usuario entidad)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
