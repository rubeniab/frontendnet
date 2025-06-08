using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using frontendnet.Models;

[Authorize(Roles = "Administrador")]
public class PedidosAdminController(PedidosClientService pedidos, IConfiguration configuration) : Controller
{
    public async Task<IActionResult> Index(string? s)
    {
        List<Pedido>? lista = [];
        try
        {
            lista = await pedidos.ObtenerPedidosAsync(); // <-- trae todos los pedidos
            if (lista == null)
                lista = [];

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