using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Models;

public class PedidoProducto
{
    // Clave primaria compuesta (PedidoId + ProductoId)
    public int PedidoId { get; set; }
    public int ProductoId { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "La cantidad debe ser entre 1 y 100")]
    public int Cantidad { get; set; }

    [Display(Name = "Precio unitario")]
    [DataType(DataType.Currency)]
    public decimal PrecioUnitario { get; set; }

    // Propiedades de navegación
    public Pedido Pedido { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}