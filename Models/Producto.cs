using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaWeb.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    [Display(Name = "Nombre del producto")]
    public string Nombre { get; set; } = "";

    [MaxLength(500)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser entre 0.01 y 999,999.99")]
    [Display(Name = "Precio (RD$)")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    [Range(0, 10000, ErrorMessage = "El stock debe ser entre 0 y 10,000")]
    [Display(Name = "Stock disponible")]
    public int Stock { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    [Required(ErrorMessage = "Debes seleccionar una categoría")]
    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }

    // Propiedades de navegación
    public Categoria? Categoria { get; set; }
    public ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>();
}