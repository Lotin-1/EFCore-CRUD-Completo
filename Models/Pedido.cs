using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaWeb.Models;

public class Pedido
{
    public int Id { get; set; }

    [Display(Name = "Fecha del pedido")]
    [DataType(DataType.DateTime)]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    [Display(Name = "Cliente")]
    public string Cliente { get; set; } = "";

    [Display(Name = "Total del pedido")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    // Propiedad de navegación: un pedido tiene muchos productos (a través de PedidoProducto)
    public ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>();
}