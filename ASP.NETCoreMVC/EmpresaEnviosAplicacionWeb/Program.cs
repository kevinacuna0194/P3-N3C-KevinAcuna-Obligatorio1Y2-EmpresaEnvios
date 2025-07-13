using Configurations;
using Microsoft.Extensions.Configuration;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Agregar configuración de ApiSettings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddTransient<ApiService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurar HttpClientService como servicio inyectable
builder.Services.AddHttpClient<HttpClientService>();

// Configurar HttpContextAccessor para acceder al contexto HTTP
builder.Services.AddHttpContextAccessor();

// Configurar AgenciaService como servicio inyectable
builder.Services.AddScoped<AgenciaService>();

// Configurar RolService como servicio inyectable
builder.Services.AddScoped<RolService>();

// Configurar UsuarioService como servicio inyectable
builder.Services.AddScoped<UsuarioService>();

// Configurar ObtenerNombreRolPorIdAsync como servicio inyectable
builder.Services.AddScoped<NombreRolPorIdService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
