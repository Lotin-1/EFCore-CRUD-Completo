using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Data;
using TiendaWeb.Models;

namespace TiendaWeb.Controllers;

public class PedidoController : Controller
{
    private readonly TiendaContext _context;

    public PedidoController(TiendaContext context)
    {
        _context = context;
    }

    // GET: Pedido
    public async Task<IActionResult> Index()
    {
        var pedidos = await _context.Pedidos
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();
        return View(pedidos);
    }

    // GET: Pedido/Detalle/5
    public async Task<IActionResult> Detalle(int? id)
    {
        if (id == null) return NotFound();

        var pedido = await _context.Pedidos
            .Include(p => p.PedidoProductos)
                .ThenInclude(pp => pp.Producto)
                    .ThenInclude(pr => pr.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null) return NotFound();

        return View(pedido);
    }

    // GET: Pedido/Crear
    public async Task<IActionResult> Crear()
    {
        ViewBag.Productos = await _context.Productos
            .Where(p => p.Activo && p.Stock > 0)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return View();
    }

    // POST: Pedido/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear([Bind("Cliente")] Pedido pedido, int[] productoIds, int[] cantidades)
    {
        if (ModelState.IsValid)
        {
            pedido.Fecha = DateTime.Now;
            pedido.Total = 0;

            // Agregar productos al pedido
            for (int i = 0; i < productoIds.Length; i++)
            {
                if (cantidades[i] <= 0) continue;

                var producto = await _context.Productos.FindAsync(productoIds[i]);
                if (producto != null && producto.Stock >= cantidades[i])
                {
                    var pedidoProducto = new PedidoProducto
                    {
                        ProductoId = producto.Id,
                        Cantidad = cantidades[i],
                        PrecioUnitario = producto.Precio
                    };

                    pedido.PedidoProductos.Add(pedidoProducto);
                    pedido.Total += producto.Precio * cantidades[i];

                    // Actualizar stock
                    producto.Stock -= cantidades[i];
                }
            }

            if (pedido.PedidoProductos.Any())
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Pedido creado correctamente";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Debe agregar al menos un producto");
        }

      // GET: Pedido/Crear
    ViewBag.Productos = await _context.Productos
        .Include(p => p.Categoria)
        .Where(p => p.Activo)
        .OrderBy(p => p.Nombre)
        .ToListAsync();
    
    return View();
}
}