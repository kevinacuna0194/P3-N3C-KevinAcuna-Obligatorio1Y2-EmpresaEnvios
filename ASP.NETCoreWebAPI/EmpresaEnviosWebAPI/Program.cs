using CasosUso.InterfacesCasosUso;
using EmpresaEnviosWebAPI.Services;
using LogicaAccesoDatos.EntityFramework;
using LogicaAccesoDatos.Repositorios;
using LogicaAplicacion.CasosUso;
using LogicaNegocio.InterfacesRepositorios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

#region Builder Services Usuario
//builder.Services.AddSingleton<IRepositorioUsuario, RepositorioUsuarioMemoria>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuarioEF>();
builder.Services.AddScoped<IAgregarUsuario, AgregarUsuario>();
builder.Services.AddScoped<IActualizarUsuario, ActualizarUsuario>();
builder.Services.AddScoped<IBuscarUsuarioPorId, BuscarUsuarioPorId>();
builder.Services.AddScoped<IEliminarUsuario, EliminarUsuario>();
builder.Services.AddScoped<IListaUsuarios, ListaUsuarios>();
builder.Services.AddScoped<IBuscarUsuarioPorEmail, BuscarUsuarioPorEmail>();
builder.Services.AddScoped<IBuscarUsuarioPorEmailYPassword, BuscarUsuarioPorEmailYPassword>();
#endregion

#region Builder Services Rol
builder.Services.AddScoped<IRepositorioRol, RepositorioRolEF>();
builder.Services.AddScoped<IListaRoles, ListaRoles>();
builder.Services.AddScoped<IBuscarNombreRolPorId, BuscarNombreRolPorId>();
#endregion

#region Builder Services Auditoria
builder.Services.AddScoped<IRepositorioAuditoria, RepositorioAuditoriaEF>();
builder.Services.AddScoped<IListaAuditorias, ListaAuditorias>();
builder.Services.AddScoped<IBuscarAuditoriaPorId, BuscarAuditoriaPorId>();
#endregion

#region Builder Services Envio
builder.Services.AddScoped<IRepositorioEnvio, RepositorioEnvioEF>();
builder.Services.AddScoped<IAgregarEnvio, AgregarEnvio>();
builder.Services.AddScoped<IActualizarEstado, ActualizarEstado>();
builder.Services.AddScoped<IBuscarEnvioPorId, BuscarEnvioPorId>();
builder.Services.AddScoped<IEliminarEnvio, EliminarEnvio>();
builder.Services.AddScoped<IListaEnvios, ListaEnvios>();
builder.Services.AddScoped<IListaEnviosComunes, ListaEnviosComunes>();
builder.Services.AddScoped<IListaEnviosUrgentes, ListaEnviosUrgentes>();
builder.Services.AddScoped<IBuscarEnviosPorTipoYEstado, BuscarEnviosPorTipoYEstado>();
builder.Services.AddScoped<IFinalizarEnvio, FinalizarEnvio>();
builder.Services.AddScoped<IBuscarEnvioPorNumeroTracking, BuscarEnvioPorNumeroTracking>();
builder.Services.AddScoped<IBuscarEnviosPorCliente, BuscarEnviosPorCliente>();
builder.Services.AddScoped<IBuscarEnviosPorEstadoYRangoFechas, BuscarEnviosPorEstadoYRangoFechas>();
builder.Services.AddScoped<IBuscarEnviosPorComentario, BuscarEnviosPorComentario>();
#endregion

#region Builder Services Agencia
builder.Services.AddScoped<IRepositorioAgencia, RepositorioAgenciaEF>();
builder.Services.AddScoped<IListaAgencias, ListaAgencias>();
builder.Services.AddScoped<IBuscarAgenciaPorId, BuscarAgenciaPorId>();
#endregion

#region Builder Services Seguimiento
builder.Services.AddScoped<IRepositorioSeguimiento, RepositorioSeguimientoEF>();
builder.Services.AddScoped<IBuscarSeguimientosPorEnvio, BuscarSeguimientosPorEnvio>();
builder.Services.AddScoped<IBuscarSeguimientosPorCliente, BuscarSeguimientosPorCliente>();
#endregion

// Configuración de Entity Framework Core con SQL Server
builder.Services.AddDbContext<LibraryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    var archivo = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var ruta = Path.Combine(AppContext.BaseDirectory, archivo);
    options.IncludeXmlComments(ruta);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Cargar la clave desde configuración
string? secureKey = builder.Configuration.GetValue<string>("Jwt:SecretKey");

// Validar que la clave JWT esté configurada correctamente
if (string.IsNullOrWhiteSpace(secureKey))
{
    throw new InvalidOperationException("La clave JWT no está configurada correctamente.");
}

// Configuración de autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Autenticación híbrida
})
.AddCookie(cookieOptions =>
{
    cookieOptions.LoginPath = "/Home/Login";
    cookieOptions.AccessDeniedPath = "/Home/Login";
    cookieOptions.LogoutPath = "/Home/CerrarSesion";
    cookieOptions.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Expiración controlada
    cookieOptions.SlidingExpiration = true; // Renueva tiempo de vida si hay actividad

})
.AddJwtBearer(jwtOptions =>
{
    jwtOptions.RequireHttpsMetadata = false;
    jwtOptions.SaveToken = true;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = ClaimTypes.Role,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Define el Issuer explícitamente
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],  // Define la audiencia correcta
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddScoped<TokenService>();

builder.Services.AddDistributedMemoryCache(); // Usa memoria para almacenar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthentication(); // Primero autenticación

app.UseAuthorization();  // Luego autorización

app.MapControllers(); // Mapea los controladores

app.Run();

