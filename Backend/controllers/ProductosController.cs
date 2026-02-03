using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers;

[Route("api/[controller]")] // La ruta será: api/productos
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly AppDbContext _context;

    // Inyección de Dependencias: El constructor recibe la base de datos lista para usar
    public ProductosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    {
        // Esto hace el "SELECT * FROM Productos" automáticamente
        return await _context.Productos.ToListAsync();
    }

    // POST: api/productos
    [HttpPost]
    public async Task<ActionResult<Producto>> PostProducto(Producto producto)
    {
        // Asignamos la fecha actual en C# para que no sea año 0001
        producto.FechaCreacion = DateTime.Now;

        // 1. Agregamos el producto a la memoria de EF Core
        _context.Productos.Add(producto);
        
        // 2. Guardamos los cambios en la base de datos SQL (Aquí se genera el ID automático)
        await _context.SaveChangesAsync();

        // 3. Devolvemos el producto creado con su nuevo ID
        return CreatedAtAction(nameof(GetProductos), new { id = producto.ID }, producto);
    }
}