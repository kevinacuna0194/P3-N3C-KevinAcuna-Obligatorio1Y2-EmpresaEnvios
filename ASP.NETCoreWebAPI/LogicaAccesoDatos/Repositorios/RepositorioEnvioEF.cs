using Enum;
using ExcepcionesPropias;
using LogicaAccesoDatos.EntityFramework;
using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.InterfacesRepositorios;
using LogicaNegocio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LogicaAccesoDatos.Repositorios
{
    public class RepositorioEnvioEF : IRepositorioEnvio
    {
        // Implementación de Entity Framework para interactuar con la base de datos
        public LibraryContext LibraryContext { get; set; }

        public RepositorioEnvioEF(LibraryContext libraryContext)
        {
            LibraryContext = libraryContext;
        }

        public void Add(Envio envio, int idUsuarioLogueado)
        {
            if (envio is null)
            {
                throw new DatosInvalidosException("El Envio no puede ser nulo.");
            }

            switch (envio)
            {
                case EnvioComun envioComun:
                    if (envioComun.Agencia == null)
                    {
                        throw new DatosInvalidosException("La Agencia no puede ser nula.");
                    }
                    LibraryContext.Entry(envioComun.Agencia).State = EntityState.Unchanged;
                    break;

                // Agrega más tipos específicos si es necesario
                default:
                    // Manejo por defecto para la clase base `Envio`
                    break;
            }

            // Verificar si ya existe un número de tracking igual
            bool existeTracking = LibraryContext.Envios
                .Any(e => e.NumeroTracking.NumeroTracking == envio.NumeroTracking.NumeroTracking);

            if (existeTracking)
            {
                // Ya existe un envío con ese número de tracking
                throw new DatosInvalidosException("El número de tracking ya existe en la base de datos.");
            }

            LibraryContext.Entry(envio.Empleado).State = EntityState.Unchanged;
            LibraryContext.Entry(envio.Cliente).State = EntityState.Unchanged;
            LibraryContext.Add(envio);
            LibraryContext.SaveChanges();

            // Verificar si el usuario fue creado correctamente
            bool exist = Existe(envio.Id);

            if (!exist)
            {
                throw new ArgumentException("El Envio no se pudo crear.");
            }

            // Obtener la fecha y hora actual en UTC
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo uruguayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Montevideo Standard Time");
            DateTime uruguayTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, uruguayTimeZone);

            // Se crea el seguimiento
            if (exist)
            {
                Seguimiento seguimiento = new Seguimiento
                {
                    Envio = envio,
                    Fecha = new FechaSeguimiento { Fecha = uruguayTime },
                    Empleado = envio.Empleado,
                    Comentario = new ComentarioSeguimiento { Comentario = $"Envío creado. Seguimiento logístico iniciado. Estado: {envio.Estado}." }
                };

                // Guardar el seguimiento                
                LibraryContext.Entry(envio).State = EntityState.Unchanged;
                LibraryContext.Entry(seguimiento.Empleado).State = EntityState.Unchanged;
                LibraryContext.Seguimientos.Add(seguimiento);
            }

            // Verificar si EntityFramework esta haciendo seguimiento al usuario
            var usuarioExistente = LibraryContext.Usuarios.Local.FirstOrDefault(u => u.Id == idUsuarioLogueado);

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
                    Accion = new AccionAuditoria { Accion = "Crear" },
                    Fecha = new FechaAuditoria { Fecha = uruguayTime },
                    Usuario = usuarioExistente,
                    Detalle = new DetalleAuditoria { Detalle = $"Se creó el envio con ID: {envio.Id}" }
                };

                // Guardar la auditoría
                LibraryContext.Auditorias.Add(auditoria);
                LibraryContext.SaveChanges();
            }
        }

        public void Update(Envio envio, int idUsuarioLogueado)
        {
            if (envio is null)
            {
                throw new DatosInvalidosException("El Envio no puede ser nulo.");
            }

            if (envio.Id <= 0)
            {
                throw new DatosInvalidosException("El ID del envío no puede ser menor o igual a cero.");
            }

            Envio shipment = FindById(envio.Id);

            if (shipment is null)
            {
                throw new DatosInvalidosException("El Envio no existe en la base de datos.");
            }

            if (shipment.NumeroTracking.NumeroTracking != envio.NumeroTracking.NumeroTracking)
            {
                Envio shipping = ObtenerEnvioPorNumeroTracking(envio.NumeroTracking.NumeroTracking);

                if (shipping != null)
                {
                    throw new DatosInvalidosException("El número de seguimiento ya existe en la base de datos.");
                }
            }

            // Verificar si el envío ya está en el contexto local
            var trackedEnvio = LibraryContext.ChangeTracker
                .Entries<Envio>()
                .FirstOrDefault(e => e.Entity.Id == envio.Id);

            if (trackedEnvio != null)
            {
                // Si ya está siendo rastreado, desvincularlo antes de actualizar
                trackedEnvio.State = EntityState.Detached;
            }

            // Desvincular entidades relacionadas antes de actualizar
            LibraryContext.Entry(envio.Empleado).State = EntityState.Detached;
            LibraryContext.Entry(envio.Cliente).State = EntityState.Detached;

            // Actualizar el envío
            LibraryContext.Entry(envio).State = EntityState.Modified;

            if (envio.Estado == EstadoEnvio.Finalizado)
            {
                envio.FechaEntrega = new FechaEntregaEnvio { FechaEntrega = DateTime.Now };

                if (envio is EnvioUrgente)
                {
                    ((EnvioUrgente)envio).CalcularEntregaEficiente();
                }

            }

            LibraryContext.SaveChanges();

            // Verificar si el envío fue actualizado correctamente
            bool exist = Existe(envio.Id);

            if (!exist)
            {
                throw new DatosInvalidosException("El Envio no se pudo actualizar.");
            }

            // Obtener la fecha y hora actual en UTC
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo uruguayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Montevideo Standard Time");
            DateTime uruguayTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, uruguayTimeZone);

            // Verificar si EntityFramework esta haciendo seguimiento al usuario
            var usuarioExistente = LibraryContext.Usuarios.Local.FirstOrDefault(u => u.Id == idUsuarioLogueado);

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
                    Detalle = new DetalleAuditoria { Detalle = $"Se actualizó el envio con ID: {envio.Id}, Número de Tracking: {envio.NumeroTracking.NumeroTracking}, Estado: {envio.Estado}" }
                };

                // Guardar la auditoría
                LibraryContext.Entry(auditoria.Usuario).State = EntityState.Unchanged;
                LibraryContext.Auditorias.Add(auditoria);
                LibraryContext.SaveChanges();
            }
        }

        public void Remove(int id, int idUsuarioLogueado)
        {
            if (id <= 0)
            {
                throw new DatosInvalidosException("El ID del envío no puede ser menor o igual a cero.");
            }

            if (idUsuarioLogueado <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario no puede ser menor o igual a cero.");
            }

            Envio envio = FindById(id);

            if (envio is null)
            {
                throw new DatosInvalidosException("El Envio no existe en la base de datos.");
            }

            // Eliminar el envío de la base de datos
            LibraryContext.Remove(envio);
            LibraryContext.Entry(envio).State = EntityState.Deleted;
            LibraryContext.SaveChanges();

            // Obtener la fecha y hora actual en UTC
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo uruguayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Montevideo Standard Time");
            DateTime uruguayTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, uruguayTimeZone);

            // Verificar si EntityFramework esta haciendo seguimiento al usuario
            var usuarioExistente = LibraryContext.Usuarios.Local.FirstOrDefault(u => u.Id == idUsuarioLogueado);

            // Si no está en el contexto local, crear una nueva instancia
            if (usuarioExistente == null)
            {
                usuarioExistente = new Usuario { Id = idUsuarioLogueado };
                LibraryContext.Attach(usuarioExistente);
            }

            // Crear la auditoría
            Auditoria auditoria = new Auditoria
            {
                Accion = new AccionAuditoria { Accion = "Eliminar" },
                Fecha = new FechaAuditoria { Fecha = uruguayTime },
                Usuario = usuarioExistente,
                Detalle = new DetalleAuditoria { Detalle = $"Se eliminó el envio con ID: {envio.Id}" }
            };

            // Guardar la auditoría
            LibraryContext.Entry(auditoria.Usuario).State = EntityState.Unchanged;
            LibraryContext.Auditorias.Add(auditoria);
            LibraryContext.SaveChanges();
        }

        public Envio ObtenerEnvioPorNumeroTracking(string numeroTracking)
        {
            if (string.IsNullOrEmpty(numeroTracking))
            {
                throw new DatosInvalidosException("El número de seguimiento no puede ser nulo o vacío.");
            }

            var envio = LibraryContext.Envios
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .FirstOrDefault(e => e.NumeroTracking.NumeroTracking == numeroTracking);

            if (envio is EnvioComun)
            {
                envio = FindById(envio.Id, TipoEnvio.Comun);
            }

            return envio;
        }

        public Envio FindById(int id, TipoEnvio tipoEnvio)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del envío no puede ser menor o igual a cero.");
            }

            switch (tipoEnvio)
            {
                case TipoEnvio.Comun:
                    return LibraryContext.Envios
                        .OfType<EnvioComun>()
                        .Include(e => e.Cliente)
                        .Include(e => e.Empleado)
                        .Include(e => e.Seguimientos)
                        .Include(e => e.Agencia)
                        .FirstOrDefault(e => e.Id == id);

                case TipoEnvio.Urgente:
                    return LibraryContext.Envios
                        .OfType<EnvioUrgente>()
                        .Include(e => e.Cliente)
                        .Include(e => e.Empleado)
                        .Include(e => e.Seguimientos)
                        .Include(e => e.DireccionPostal)
                        .FirstOrDefault(e => e.Id == id);

                default:
                    return LibraryContext.Envios
                        .Include(e => e.Cliente)
                        .Include(e => e.Empleado)
                        .Include(e => e.Seguimientos)
                        .FirstOrDefault(e => e.Id == id);
            }
        }

        public List<Envio> FindAll()
        {
            return LibraryContext.Envios
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .OrderBy(e => e.Id)
                .ToList();
        }

        public IEnumerable<EnvioComun> ObtenerEnviosComunes()
        {
            return LibraryContext.Envios
                .OfType<EnvioComun>()
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .Include(e => e.Agencia)
                .OrderBy(e => e.Id)
                .ToList();
        }

        public IEnumerable<EnvioUrgente> ObtenerEnviosUrgentes()
        {
            return LibraryContext.Envios
                .OfType<EnvioUrgente>()
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .Include(e => e.DireccionPostal)
                .OrderBy(e => e.Id)
                .ToList();
        }

        public IEnumerable<Envio> FindAll(TipoEnvio tipoEnvio)
        {
            switch (tipoEnvio)
            {
                case TipoEnvio.Comun:
                    return ObtenerEnviosComunes().Cast<Envio>().ToList();

                case TipoEnvio.Urgente:
                    return ObtenerEnviosUrgentes().Cast<Envio>().ToList();

                case TipoEnvio.Todos:
                default:
                    return LibraryContext.Envios
                        .Include(e => e.Cliente)
                        .Include(e => e.Empleado)
                        .Include(e => e.Seguimientos)
                        .OrderBy(e => e.Id)
                        .ToList();
            }
        }
        public bool Existe(int id)
        {
            return LibraryContext.Envios.Any(e => e.Id == id);
        }

        public IEnumerable<Envio> ObtenerEnviosPorEstado(EstadoEnvio estado)
        {
            return LibraryContext.Envios
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .Where(e => e.Estado == estado)
                .ToList();
        }

        public void FinalizarEnvio(Envio envio, int idUsuarioLogueado)
        {
            if (envio is null)
            {
                throw new DatosInvalidosException("El Envio no puede ser nulo.");
            }

            if (idUsuarioLogueado <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario no puede ser menor o igual a cero.");
            }

            // Verificar si el envío ya está finalizado
            envio.FinalizarEnvio();

            // Guardar el envío actualizado
            Update(envio, idUsuarioLogueado);

            bool exist = Existe(envio.Id);

            if (!exist)
            {
                throw new DatosInvalidosException("El Envio no se pudo finalizar.");
            }

            // Obtener la fecha y hora actual en UTC
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo uruguayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Montevideo Standard Time");
            DateTime uruguayTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, uruguayTimeZone);

            // Verificar si Empleado ya está en el contexto local y desvincularlo si es necesario
            var empleadoExistente = LibraryContext.ChangeTracker
                .Entries<Usuario>()
                .FirstOrDefault(e => e.Entity.Id == envio.Empleado.Id);

            if (empleadoExistente != null)
            {
                empleadoExistente.State = EntityState.Detached;
            }

            // Crear seguimiento
            if (exist)
            {
                Seguimiento seguimiento = new Seguimiento
                {
                    Envio = envio,
                    Fecha = new FechaSeguimiento { Fecha = uruguayTime },
                    Empleado = envio.Empleado,
                    Comentario = new ComentarioSeguimiento { Comentario = $"Envío finalizado. ID: {envio.Id}, Número de Tracking: {envio.NumeroTracking.NumeroTracking}." }
                };

                // Guardar el seguimiento                
                LibraryContext.Entry(envio).State = EntityState.Unchanged;
                LibraryContext.Entry(seguimiento.Empleado).State = EntityState.Unchanged;
                LibraryContext.Seguimientos.Add(seguimiento);
                LibraryContext.SaveChanges();
            }
        }

        public void ActualizarEstado(Envio envio, int idUsuarioLogueado, string comentario)
        {
            if (envio is null)
            {
                throw new DatosInvalidosException("El Envio no puede ser nulo.");
            }

            if (idUsuarioLogueado <= 0)
            {
                throw new DatosInvalidosException("El ID del usuario no puede ser menor o igual a cero.");
            }

            if (String.IsNullOrEmpty(comentario))
            {
                throw new DatosInvalidosException("El comentario no puede ser nulo o vacío.");
            }

            // Guardar el envío actualizado
            Update(envio, idUsuarioLogueado);

            bool exist = Existe(envio.Id);

            if (!exist)
            {
                throw new DatosInvalidosException("El Envio no se pudo actualizar.");
            }

            // Obtener la fecha y hora actual en UTC
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo uruguayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Montevideo Standard Time");
            DateTime uruguayTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, uruguayTimeZone);

            // Verificar si Empleado ya está en el contexto local y desvincularlo si es necesario
            var empleadoExistente = LibraryContext.ChangeTracker
                .Entries<Usuario>()
                .FirstOrDefault(e => e.Entity.Id == envio.Empleado.Id);

            if (empleadoExistente != null)
            {
                empleadoExistente.State = EntityState.Detached;
            }

            // Crear el seguimiento con la referencia correcta de 'Empleado'
            Seguimiento seguimiento = new Seguimiento
            {
                Envio = envio,
                Fecha = new FechaSeguimiento { Fecha = DateTime.UtcNow },
                Empleado = envio.Empleado,
                Comentario = new ComentarioSeguimiento
                {
                    Comentario = !string.IsNullOrWhiteSpace(comentario) ? comentario : $"Envío Actualizado. ID: {envio.Id}, Número de Tracking: {envio.NumeroTracking.NumeroTracking}."
                }
            };

            // Desvincular Envio antes de guardarlo para evitar conflictos de rastreo
            LibraryContext.Entry(envio).State = EntityState.Unchanged;
            LibraryContext.Entry(seguimiento.Empleado).State = EntityState.Unchanged;

            // Guardar el seguimiento
            LibraryContext.Seguimientos.Add(seguimiento);
            LibraryContext.SaveChanges();
        }

        public IEnumerable<Envio> ObtenerEnviosPorCliente(int idCliente)
        {
            if (idCliente <= 0)
                throw new DatosInvalidosException("El ID del cliente no puede ser menor o igual a cero.");

            var enviosComunes = LibraryContext.Envios
                .OfType<EnvioComun>()
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .Include(e => e.Agencia)
                .Where(e => e.Cliente.Id == idCliente)
                .ToList();

            var enviosUrgentes = LibraryContext.Envios
                .OfType<EnvioUrgente>()
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .Include(e => e.DireccionPostal)
                .Where(e => e.Cliente.Id == idCliente)
                .ToList();

            return enviosComunes.Cast<Envio>()
                .Concat(enviosUrgentes.Cast<Envio>())
                .OrderBy(e => e.Id)
                .ToList();
        }

        public IEnumerable<Envio> ObtenerEnviosPorEmpleado(int idEmpleado)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorFecha(DateTime fecha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorTipo(TipoEnvio tipoEnvio)
        {
            return tipoEnvio switch
            {
                TipoEnvio.Comun => ObtenerEnviosComunes().Cast<Envio>().ToList(),
                TipoEnvio.Urgente => ObtenerEnviosUrgentes().Cast<Envio>().ToList(),
                TipoEnvio.Todos => LibraryContext.Envios
                    .Include(e => e.Cliente)
                    .Include(e => e.Empleado)
                    .Include(e => e.Seguimientos)
                    .ToList(),
                _ => throw new DatosInvalidosException("Tipo de envío no válido.")
            };
        }

        public IEnumerable<Envio> ObtenerEnviosPorPeso(decimal pesoMinimo, decimal pesoMaximo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorDireccion(string direccion)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorPrioridad(string prioridad)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorTipoYEstado(TipoEnvio tipoEnvio, EstadoEnvio estado)
        {
            return tipoEnvio switch
            {
                TipoEnvio.Comun => ObtenerEnviosComunes().Where(e => e.Estado == estado).Cast<Envio>().ToList(),
                TipoEnvio.Urgente => ObtenerEnviosUrgentes().Where(e => e.Estado == estado).Cast<Envio>().ToList(),
                TipoEnvio.Todos => LibraryContext.Envios
                    .Include(e => e.Cliente)
                    .Include(e => e.Empleado)
                    .Include(e => e.Seguimientos)
                    .Where(e => e.Estado == estado)
                    .ToList(),
                _ => throw new DatosInvalidosException("Tipo de envío no válido.")
            };
        }

        public IEnumerable<Envio> ObtenerEnviosPorTipoYFecha(TipoEnvio tipoEnvio, DateTime fecha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorTipoYRangoFechas(TipoEnvio tipoEnvio, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorEstadoYFecha(EstadoEnvio estado, DateTime fecha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorEstadoYRangoFechas(EstadoEnvio estado, DateTime fechaInicio, DateTime fechaFin, int idCliente)
        {
            // Envíos Comunes
            var enviosComunesBase = LibraryContext.Envios
                .OfType<EnvioComun>()
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .Include(e => e.Agencia)
                .Where(e => e.Cliente.Id == idCliente)
                .AsQueryable();

            // Envíos Urgentes
            var enviosUrgentesBase = LibraryContext.Envios
                .OfType<EnvioUrgente>()
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .Include(e => e.DireccionPostal)
                .Where(e => e.Cliente.Id == idCliente)
                .AsQueryable();

            IQueryable<EnvioComun> enviosComunesQuery;
            IQueryable<EnvioUrgente> enviosUrgentesQuery;

            if (estado == EstadoEnvio.Finalizado)
            {
                enviosComunesQuery = enviosComunesBase.Where(e =>
                    e.Estado == EstadoEnvio.Finalizado &&
                    e.FechaEntrega != null &&
                    e.FechaEntrega.FechaEntrega >= fechaInicio &&
                    e.FechaEntrega.FechaEntrega <= fechaFin);

                enviosUrgentesQuery = enviosUrgentesBase.Where(e =>
                    e.Estado == EstadoEnvio.Finalizado &&
                    e.FechaEntrega != null &&
                    e.FechaEntrega.FechaEntrega >= fechaInicio &&
                    e.FechaEntrega.FechaEntrega <= fechaFin);
            }
            else if (estado == EstadoEnvio.Todos)
            {
                var comunesFinalizados = enviosComunesBase.Where(e =>
                    e.Estado == EstadoEnvio.Finalizado &&
                    e.FechaEntrega != null &&
                    e.FechaEntrega.FechaEntrega >= fechaInicio &&
                    e.FechaEntrega.FechaEntrega <= fechaFin);

                var comunesNoFinalizados = enviosComunesBase.Where(e =>
                    e.Estado != EstadoEnvio.Finalizado &&
                    e.FechaSalida.FechaSalida >= fechaInicio &&
                    e.FechaSalida.FechaSalida <= fechaFin);

                enviosComunesQuery = comunesFinalizados.Concat(comunesNoFinalizados);

                var urgentesFinalizados = enviosUrgentesBase.Where(e =>
                    e.Estado == EstadoEnvio.Finalizado &&
                    e.FechaEntrega != null &&
                    e.FechaEntrega.FechaEntrega >= fechaInicio &&
                    e.FechaEntrega.FechaEntrega <= fechaFin);

                var urgentesNoFinalizados = enviosUrgentesBase.Where(e =>
                    e.Estado != EstadoEnvio.Finalizado &&
                    e.FechaSalida.FechaSalida >= fechaInicio &&
                    e.FechaSalida.FechaSalida <= fechaFin);

                enviosUrgentesQuery = urgentesFinalizados.Concat(urgentesNoFinalizados);
            }
            else
            {
                enviosComunesQuery = enviosComunesBase.Where(e =>
                    e.Estado == estado &&
                    e.FechaSalida.FechaSalida >= fechaInicio &&
                    e.FechaSalida.FechaSalida <= fechaFin);

                enviosUrgentesQuery = enviosUrgentesBase.Where(e =>
                    e.Estado == estado &&
                    e.FechaSalida.FechaSalida >= fechaInicio &&
                    e.FechaSalida.FechaSalida <= fechaFin);
            }

            var enviosComunes = enviosComunesQuery.ToList();
            var enviosUrgentes = enviosUrgentesQuery.ToList();

            return enviosComunes.Cast<Envio>()
                .Concat(enviosUrgentes.Cast<Envio>())
                .OrderBy(e => e.NumeroTracking.NumeroTracking)
                .ToList();
        }

        public IEnumerable<Envio> ObtenerEnviosPorClienteYEstado(int idCliente, EstadoEnvio estado)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorClienteYFecha(int idCliente, DateTime fecha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorClienteYRangoFechas(int idCliente, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorEmpleadoYEstado(int idEmpleado, EstadoEnvio estado)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorEmpleadoYFecha(int idEmpleado, DateTime fecha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Envio> ObtenerEnviosPorEmpleadoYRangoFechas(int idEmpleado, DateTime fechaInicio, DateTime fechaFin)
        {
            throw new NotImplementedException();
        }

        public void Add(Envio entidad)
        {
            throw new NotImplementedException();
        }

        public void Update(Envio entidad)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Envio FindById(int id)
        {
            return LibraryContext.Envios
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Envio> ObtenerEnviosPorComentario(string comentario, int idCliente)
        {
            if (string.IsNullOrWhiteSpace(comentario))
            {
                throw new ArgumentNullException(nameof(comentario), "El comentario no puede ser nulo ni estar vacío.");
            }

            var comentarioLower = comentario.ToLower();

            var enviosComunesQuery = LibraryContext.Envios
                .OfType<EnvioComun>()
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .Include(e => e.Agencia)
                .Where(e => e.Cliente.Id == idCliente &&
                    e.Seguimientos.Any(s =>
                        s.Comentario.Comentario != null &&
                        s.Comentario.Comentario.ToLower().Contains(comentarioLower)));

            var enviosUrgentesQuery = LibraryContext.Envios
                .OfType<EnvioUrgente>()
                .Include(e => e.Cliente)
                .Include(e => e.Empleado)
                .Include(e => e.Seguimientos)
                .Include(e => e.DireccionPostal)
                .Where(e => e.Cliente.Id == idCliente &&
                    e.Seguimientos.Any(s =>
                        s.Comentario.Comentario != null &&
                        s.Comentario.Comentario.ToLower().Contains(comentarioLower)));

            var enviosComunes = enviosComunesQuery.ToList();
            var enviosUrgentes = enviosUrgentesQuery.ToList();

            var envios = enviosComunes.Cast<Envio>()
                .Concat(enviosUrgentes.Cast<Envio>())
                .ToList();

            return envios
                .OrderByDescending(e =>
                    e.Seguimientos
                        .Where(s => s.Comentario?.Comentario != null &&
                                    s.Comentario.Comentario.ToLower().Contains(comentarioLower))
                        .Select(s => s.Fecha.Fecha)
                        .DefaultIfEmpty(DateTime.MinValue)
                        .Max())
                .ToList();
        }
    }
}