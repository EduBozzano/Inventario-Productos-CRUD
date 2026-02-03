using Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios al contenedor (Dependency Injection)

// A. Configurar la conexión a SQL Server leyendo del appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// B. Agregar los controladores (para que funcionen las APIs)
builder.Services.AddControllers();

// C. Configurar Swagger (la pantalla de ayuda para probar APIs)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// D. Configurar CORS (IMPORTANTE: Esto permite que Vue desde el puerto 5173 hable con .NET)
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app => 
    {
        app.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

var app = builder.Build();

// 2. Configurar el pipeline de peticiones HTTP

// Si estamos en desarrollo, usamos Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Activar la política de CORS que creamos arriba
app.UseCors("NuevaPolitica");

app.UseAuthorization();

app.MapControllers();

app.Run();
