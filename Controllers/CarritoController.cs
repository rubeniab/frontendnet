using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Usuario")]
public class CarritoController(PedidosClientService pedidos, ProductosClientService productos) : Controller
{
    private readonly ProductosClientService productos = productos;
    // Muestra el carrito actual del usuario
    // Muestra el carrito actual del usuario
    public async Task<IActionResult> Index()
    {
        var email = User.Identity?.Name!;
        List<ItemPedido>? items = null;
        try
        {
            items = await pedidos.ObtenerItemsCarritoAsync(email);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
            return View(new Pedido { Items = new List<ItemPedido>() });
        }
        catch (Exception)
        {
            return View(new Pedido { Items = new List<ItemPedido>() });
        }

        ViewBag.Url = "http://localhost:3000";
        var total = items?.Sum(i => i.Subtotal) ?? 0;
        return View(new Pedido { Items = items ?? new List<ItemPedido>(), Total = total });
    }

    [HttpPost]
    public async Task<IActionResult> EliminarDelCarrito(int itemId)
    {
        var email = User.Identity?.Name!;
        await pedidos.EliminarDelCarritoAsync(email, itemId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ModificarCantidad(int itemId, int cantidad)
    {
        var email = User.Identity?.Name!;
        await pedidos.ModificarCantidadCarritoAsync(email, itemId, cantidad);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ComprarCarrito()
    {
        var email = User.Identity?.Name!;
        await pedidos.ConfirmarCarritoAsync(email);
        return RedirectToAction("Index", "Pedidos");
    }
}