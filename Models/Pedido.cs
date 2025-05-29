using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Pedido
{
    [Display(Name = "Id")]
    public int? PedidoId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public required string UsuarioId { get; set; }
}