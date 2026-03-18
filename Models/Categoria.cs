using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = "";

    [MaxLength(300, ErrorMessage = "La descripción no puede exceder 300 caracteres")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    // Propiedad de navegación: una categoría tiene muchos productos
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}