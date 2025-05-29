using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Pedido
{
    public int? Id { get; set; }

    [Required]
    public string UsuarioId { get; set; } = "";

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public string Estado { get; set; } = "pendiente";

    [Required]
    public decimal Total { get; set; }

    [Required]
    public string DireccionEnvio { get; set; } = "";

    [Required]
    public string MetodoPago { get; set; } = "";

    public bool EsCarrito { get; set; } = false;

    public List<ItemPedido>? Items { get; set; }
}