namespace Backend.Models;

// Modelo de la tabla Producto 
public class Producto
{
    public int ID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public DateTime FechaCreacion { get; set; }
}