using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class ItemPedido
{
    public int? Id { get; set; }

    [Required]
    public int PedidoId { get; set; }

    [Required]
    public int ProductoId { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Required]
    public decimal PrecioUnitario { get; set; }

    [Required]
    public decimal Subtotal { get; set; }

    public String? Titulo { get; set; }

    public String? Descripcion { get; set; }

    public int? ArchivoId { get; set; }
}