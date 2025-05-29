using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class PedidoProducto
{
    [Display(Name = "Id")]
    public int? PedidoProductoId { get; set; }

    // Foreign key to Pedido
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public required int PedidoId { get; set; }
    // Foreign key to Producto
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public required int ProductoId { get; set; }
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Cantidad")]
    public required int Cantidad { get; set; }
}