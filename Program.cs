using Microsoft.AspNetCore.Authentication.Cookies;
using repositoriosTP6;
using tl2_tp6_2024_ElZorroAs.Services;
using tl2_tp6_2024_ElZorroAs.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores con vistas
builder.Services.AddControllersWithViews();

// Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";  // Página de login
        options.AccessDeniedPath = "/Home/AccesoDenegado";  // Redirección si no tiene permisos
    });

// Configurar sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Sesión expira en 30 min
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Inyección de dependencias
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IPresupuestoRepository, PresupuestosRepository>();
builder.Services.AddScoped<IClientesRepository, ClientesRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>(); // Cambiado de Singleton a Scoped
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpContextAccessor(); // Para acceder a HttpContext en servicios

var app = builder.Build();

// Usar sesiones
app.UseSession();

// Configuración del pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();  // 🔴 Esto es lo que faltaba
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
