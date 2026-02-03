using Microsoft.EntityFrameworkCore;
using Backend.Models;

//se configura la conexion de el modelo con la BD
namespace Backend.Data;

// Se hereda de DbContext, que es una clase ya hecha en .NET para base de datos
public class AppDbContext : DbContext
{
    // Constructor que recibe las opcioones de configuracion (como cadena de conexion)
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) //basse es como el super() de java
    {
        
    }

    // Esto le dice a C#: "Hay una tabla en la DB que se debe mapear a esta lista de objetos"
    public DbSet<Producto> Productos { get; set;}
}