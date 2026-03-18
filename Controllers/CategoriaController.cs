using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Data;
using TiendaWeb.Models;

namespace TiendaWeb.Controllers;

public class CategoriaController : Controller
{
    private readonly TiendaContext _context;

    public CategoriaController(TiendaContext context)
    {
        _context = context;
    }

    // GET: Categoria
    public async Task<IActionResult> Index()
    {
        var categorias = await _context.Categorias
            .OrderBy(c => c.Nombre)
            .ToListAsync();
        return View(categorias);
    }

    // GET: Categoria/Detalle/5
    public async Task<IActionResult> Detalle(int? id)
    {
        if (id == null) return NotFound();

        var categoria = await _context.Categorias
            .Include(c => c.Productos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null) return NotFound();

        return View(categoria);
    }

    // GET: Categoria/Crear
    public IActionResult Crear()
    {
        return View();
    }

    // POST: Categoria/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            TempData["Exito"] = $"Categoría '{categoria.Nombre}' creada exitosamente";
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    // GET: Categoria/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();

        return View(categoria);
    }

    // POST: Categoria/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Categoria categoria)
    {
        if (id != categoria.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"Categoría '{categoria.Nombre}' actualizada";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Categorias.AnyAsync(c => c.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    // GET: Categoria/Eliminar/5
    public async Task<IActionResult> Eliminar(int? id)
    {
        if (id == null) return NotFound();

        var categoria = await _context.Categorias
            .Include(c => c.Productos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null) return NotFound();

        // Verificar si tiene productos asociados
        if (categoria.Productos != null && categoria.Productos.Any())
        {
            TempData["Error"] = "No se puede eliminar la categoría porque tiene productos asociados";
            return RedirectToAction(nameof(Index));
        }

        return View(categoria);
    }

    // POST: Categoria/Eliminar/5
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            TempData["Exito"] = "Categoría eliminada";
        }
        return RedirectToAction(nameof(Index));
    }
}