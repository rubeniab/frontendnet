using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Usuario")]
public class CarritoController(PedidosClientService pedidos) : Controller
{
    // Muestra el carrito actual del usuario
    public async Task<IActionResult> Index()
    {
        var email = User.Identity?.Name!;
        Pedido? carrito = null;
        try
        {
            carrito = await pedidos.ObtenerCarritoAsync(email);
            if (carrito != null && (carrito.Items == null || carrito.Items.Count == 0))
            {
                carrito.Items = await pedidos.ObtenerItemsCarritoAsync(email) ?? [];
            }
        }
        catch (HttpRequestException ex)
        {
            // Puedes loguear el error aquí si quieres
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
            // Si hay otro error, muestra una vista vacía
            return View(new Pedido { Items = new List<ItemPedido>() });
        }
        catch (Exception)
        {
            // Cualquier otro error
            return View(new Pedido { Items = new List<ItemPedido>() });
        }

        // Si usas imágenes, asigna ViewBag.Url
        ViewBag.Url = "http://localhost:3000"; // O usa configuration["UrlWebAPI"] si tienes IConfiguration

        return View(carrito ?? new Pedido { Items = new List<ItemPedido>() });
    }

    [HttpPost]
    public async Task<IActionResult> AgregarAlCarrito(int productoId, int cantidad)
    {
        var email = User.Identity?.Name!;
        await pedidos.AgregarAlCarritoAsync(email, productoId, cantidad);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> EliminarDelCarrito(int itemId)
    {
        var email = User.Identity?.Name!;
        await pedidos.EliminarDelCarritoAsync(email, itemId);
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