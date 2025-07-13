using LogicaNegocio.EntidadesDominio;
using Microsoft.EntityFrameworkCore;

namespace LogicaAccesoDatos.EntityFramework
{
    public class LibraryContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Envio> Envios { get; set; }
        public DbSet<EnvioComun> EnviosComunes { get; set; }
        public DbSet<EnvioUrgente> EnviosUrgentes { get; set; }
        public DbSet<Agencia> Agencias { get; set; }
        public DbSet<Seguimiento> Seguimientos { get; set; }

        // Conexión a la base de datos
        public LibraryContext(DbContextOptions options) : base(options)
        {
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=localhost;Database=LibraryDB;UserId=sa;Password=your_password;");
        }*/

        /* Para tener más control y asignar nombres específicos y claros, utilizamos Fluent API dentro del método OnModelCreating del   DbContext. Este método te permite configurar el modelo de tus entidades antes de que EF cree la base de datos. Veamos cómo funciona paso a paso:
          - OwnsOne Este método le dice a Entity Framework que una propiedad es owned. Puedes usarlo para definir cómo se mapearán las propiedades de esa clase owned.
          - Property Este método se utiliza para especificar una subpropiedad dentro de la propiedad owned.
          - HasColumnName Con este método, puedes asignar un nombre específico a la columna en la base de datos. */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configura la entidad principal
            modelBuilder.Entity<Usuario>(entity =>
            {
                // Configura la propiedad owned 'Nombre'
                entity.OwnsOne(e => e.Nombre, nombre =>
                {
                    // Configura la subpropiedad 'Valor'
                    nombre.Property(n => n.Nombre)
                        .HasColumnName("Nombre"); // Asigna el nombre personalizado de la columna
                });

                entity.OwnsOne(e => e.Email, email =>
                {
                    email.Property(e => e.Email)
                        .HasColumnName("Email");
                });

                entity.OwnsOne(e => e.Password, password =>
                {
                    password.Property(n => n.Password)
                        .HasColumnName("Password");
                });

                entity.OwnsOne(e => e.FechaRegistro, fechaRegistro =>
                {
                    fechaRegistro.Property(n => n.FechaRegistro)
                        .HasColumnName("FechaRegistro");
                });

                entity.OwnsOne(e => e.FechaNacimiento, fechaNacimiento =>
                {
                    fechaNacimiento.Property(n => n.FechaNacimiento)
                        .HasColumnName("FechaNacimiento");
                });

                entity.OwnsOne(e => e.Telefono, telefono =>
                {
                    telefono.Property(n => n.Telefono)
                        .HasColumnName("Telefono");
                });

                entity.OwnsOne(e => e.Direccion, direccion =>
                {
                    direccion.Property(n => n.Direccion)
                        .HasColumnName("Direccion");
                });

                entity.OwnsOne(e => e.DocumentoIdentidad, documentoIdentidad =>
                {
                    documentoIdentidad.Property(n => n.DocumentoIdentidad)
                        .HasColumnName("DocumentoIdentidad");
                });
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                // Configura la propiedad owned 'Nombre'
                entity.OwnsOne(e => e.Nombre, nombre =>
                {
                    // Configura la subpropiedad 'Valor'
                    nombre.Property(n => n.Nombre)
                        .HasColumnName("Nombre"); // Asigna el nombre personalizado de la columna
                });
            });

            modelBuilder.Entity<Auditoria>(entity =>
            {
                entity.OwnsOne(e => e.Accion, accion =>
                {
                    accion.Property(a => a.Accion)
                        .HasColumnName("Accion");
                });

                entity.OwnsOne(e => e.Fecha, fecha =>
                {
                    fecha.Property(f => f.Fecha)
                        .HasColumnName("Fecha");
                });

                entity.OwnsOne(e => e.Detalle, detalle =>
                {
                    detalle.Property(f => f.Detalle)
                        .HasColumnName("Detalle");
                });
            });

            modelBuilder.Entity<Envio>()
                .ToTable("Envios")
                .HasKey(e => e.Id); // Define la clave primaria

            modelBuilder.Entity<Envio>()
                .HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey("ClienteId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Envio>()
                .HasOne(e => e.Empleado)
                .WithMany()
                .HasForeignKey("EmpleadoId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Envio>(entity =>
            {
                entity.OwnsOne(e => e.NumeroTracking, numeroTracking =>
                {
                    numeroTracking.Property(nt => nt.NumeroTracking)
                        .HasColumnName("NumeroTracking");
                });

                entity.OwnsOne(e => e.Peso, peso =>
                {
                    peso.Property(p => p.Peso)
                        .HasColumnName("Peso");
                });

                entity.OwnsOne(e => e.FechaSalida, fechaSalida =>
                {
                    fechaSalida.Property(fs => fs.FechaSalida)
                        .HasColumnName("FechaSalida");
                });

                entity.OwnsOne(e => e.FechaEntrega, fechaEntrega =>
                {
                    fechaEntrega.Property(fe => fe.FechaEntrega)
                        .HasColumnName("FechaEntrega");
                });
            });

            modelBuilder.Entity<EnvioComun>()
                .ToTable("EnviosComunes")
                .HasBaseType<Envio>(); // Indica que EnvioComun hereda de Envio

            modelBuilder.Entity<EnvioUrgente>()
                .ToTable("EnviosUrgentes")
                .HasBaseType<Envio>(); // Indica que EnvioUrgente hereda de Envio

            modelBuilder.Entity<EnvioUrgente>(entity =>
            {
                entity.OwnsOne(e => e.DireccionPostal, direccionPostal =>
                {
                    direccionPostal.Property(dp => dp.DireccionPostal)
                        .HasColumnName("DireccionPostal");
                });
            });

            modelBuilder.Entity<Agencia>()
                .ToTable("Agencias")
                .HasKey(e => e.Id); // Define la clave primaria

            modelBuilder.Entity<Agencia>(entity =>
            {
                entity.OwnsOne(e => e.Nombre, nombre =>
                {
                    // Configura la subpropiedad 'Valor'
                    nombre.Property(n => n.Nombre)
                        .HasColumnName("Nombre"); // Asigna el nombre personalizado de la columna
                });

                entity.OwnsOne(e => e.DireccionPostal, direccionPostal =>
                {
                    direccionPostal.Property(n => n.DireccionPostal)
                        .HasColumnName("DireccionPostal");
                });

                entity.OwnsOne(e => e.Latitud, latitud =>
                {
                    latitud.Property(n => n.Latitud)
                        .HasColumnName("Latitud");
                });

                entity.OwnsOne(e => e.Longitud, longitud =>
                {
                    longitud.Property(n => n.Longitud)
                        .HasColumnName("Longitud");
                });
            });

            modelBuilder.Entity<Seguimiento>()
                .ToTable("Seguimientos")
                .HasKey(e => e.Id); // Define la clave primaria

            modelBuilder.Entity<Seguimiento>(entity =>
            {
                entity.OwnsOne(e => e.Fecha, fecha =>
                {
                    fecha.Property(f => f.Fecha)
                        .HasColumnName("Fecha");
                });
                entity.OwnsOne(e => e.Comentario, comentario =>
                {
                    comentario.Property(d => d.Comentario)
                        .HasColumnName("Comentario");
                });
            });
        }
    }
}
