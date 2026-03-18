using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Data;
using TiendaWeb.Models;

namespace TiendaWeb.Controllers;

public class ProductoController : Controller
{
    private readonly TiendaContext _context;

    public ProductoController(TiendaContext context)
    {
        _context = context;
    }

    // GET: Producto
    public async Task<IActionResult> Index()
    {
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
        return View(productos);
    }

    // GET: Producto/Detalle/5
    public async Task<IActionResult> Detalle(int? id)
    {
        if (id == null) return NotFound();

        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null) return NotFound();

        return View(producto);
    }

    // GET: Producto/Crear
    public async Task<IActionResult> Crear()
    {
        await CargarCategoriasAsync();
        return View();
    }

    // POST: Producto/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Producto producto)
    {
        // Validación personalizada: SKU único (si lo tuvieras)
        // if (await _context.Productos.AnyAsync(p => p.CodigoSku == producto.CodigoSku))
        //     ModelState.AddModelError("CodigoSku", "El SKU ya existe");

        if (ModelState.IsValid)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            TempData["Exito"] = $"Producto '{producto.Nombre}' creado";
            return RedirectToAction(nameof(Index));
        }

        await CargarCategoriasAsync(producto.CategoriaId);
        return View(producto);
    }

    // GET: Producto/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return NotFound();

        await CargarCategoriasAsync(producto.CategoriaId);
        return View(producto);
    }

    // POST: Producto/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Producto producto)
    {
        if (id != producto.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"Producto '{producto.Nombre}' actualizado";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Productos.AnyAsync(p => p.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        await CargarCategoriasAsync(producto.CategoriaId);
        return View(producto);
    }

    // GET: Producto/Eliminar/5
    public async Task<IActionResult> Eliminar(int? id)
    {
        if (id == null) return NotFound();

        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null) return NotFound();

        return View(producto);
    }

    // POST: Producto/Eliminar/5
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto != null)
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            TempData["Exito"] = "Producto eliminado";
        }
        return RedirectToAction(nameof(Index));
    }

    // Helper: cargar dropdown de categorías
    private async Task CargarCategoriasAsync(int? seleccionado = null)
    {
        var categorias = await _context.Categorias
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        ViewBag.CategoriaId = new SelectList(categorias, "Id", "Nombre", seleccionado);
    }
}