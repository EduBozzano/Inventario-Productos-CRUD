using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;

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
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos(string? busqueda)
    { 
        if (string.IsNullOrEmpty(busqueda))
        {
            // Esto hace el "SELECT * FROM Productos" automáticamente
            return await _context.Productos.ToListAsync();         
        }  
        else
        {
            return await _context.Productos.Where(product => product.Nombre.Contains(busqueda)).ToListAsync();
        }
    }

    // POST: api/productos
    [HttpPost]
    public async Task<ActionResult<Producto>> PostProducto(Producto producto)
    {
        // Asignamos la fecha actual en C# 
        producto.FechaCreacion = DateTime.Now;

        // 1. Agregamos el producto a la memoria de EF Core
        _context.Productos.Add(producto);
        
        // 2. Guardamos los cambios en la base de datos SQL (Aquí se genera el ID automático)
        await _context.SaveChangesAsync();

        // 3. Devolvemos el producto creado con su nuevo ID
        return CreatedAtAction(nameof(GetProductos), new { id = producto.ID }, producto);
    }

    // PUT: api/productos/5
    // Se usa para ACTUALIZAR. Recibe el ID en la URL y el objeto modificado en el cuerpo.
    [HttpPut("{id}")] 
    public async Task<IActionResult> PutProducto(int id, Producto producto)
    {
        if (id != producto.ID)
        {
            return BadRequest("El ID de la URL no coincide con el del producto.");
        }

        // Le decimos a EF Core: "Este objeto ya existe, pero sus valores han cambiado"
        _context.Entry(producto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Productos.Any(e => e.ID == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent(); // 204 No Content es el estándar cuando una actualización sale bien
    }

    // DELETE: api/productos/5
    // Se usa para ELIMINAR. Solo necesita el ID en la URL.
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProducto(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound();
        }

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}