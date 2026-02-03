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
}