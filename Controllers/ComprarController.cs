using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Usuario")]
public class ComprarController(ProductosClientService productos, PedidosClientService pedidos) : Controller
{
    public async Task<IActionResult> Index(string? s)
    {
        var lista = await productos.GetAsync(s);
        ViewBag.Url = "http://localhost:3000";
        return View(lista ?? []);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var producto = await productos.GetAsync(id);
        if (producto == null) return NotFound();
        ViewBag.Url = "http://localhost:3000";
        return View(producto);
    }

    [HttpPost]
    public async Task<IActionResult> ComprarAhora(int productoId, int cantidad, string direccionEnvio, string metodoPago)
    {
        var email = User.Identity?.Name!;
        await pedidos.ComprarAhoraAsync(email, productoId, cantidad, direccionEnvio, metodoPago);
        return RedirectToAction("Index", "Pedidos");
    }

    [HttpPost]
    public async Task<IActionResult> AgregarAlCarrito(int productoId, int cantidad)
    {
        var email = User.Identity?.Name!;
        await pedidos.AgregarAlCarritoAsync(email, productoId, cantidad);
        return RedirectToAction("Index", "Carrito");
    }
}