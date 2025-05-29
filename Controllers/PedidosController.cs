using frontendnet.Models;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

public class PedidosController(PedidosClientService pedidos, IConfiguration configuration) : Controller
{
    public async Task<IActionResult> Index(string? s)
    {
        List<Pedido>? lista = [];

        var email = User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Salir", "Auth");
        }

        try
        {
            lista = await pedidos.ObtenerPedidosAsync();
            if (lista == null)
                lista = [];

            // Si hay filtro
            if (!string.IsNullOrEmpty(s))
            {
                lista = lista.Where(p => p.UsuarioId.Contains(s, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        ViewBag.Url = configuration["UrlWebAPI"];
        ViewBag.search = s;

        return View(lista);
    }
}
