using RelojMarcador_ModuloUsuarios.Repository;
using RelojMarcador_ModuloUsuarios.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("MySql")
    ?? throw new InvalidOperationException("Cadena de conexión no encontrada.");

// Inyectamos repositorio y servicio
builder.Services.AddScoped<IMarcaRepository>(sp => new MarcaRepository(connectionString));
builder.Services.AddScoped<IMarcaService, MarcaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// Redirige a /Marcas por defecto
app.MapGet("/", context =>
{
    context.Response.Redirect("/Marcas");
    return Task.CompletedTask;
});

app.Run();


