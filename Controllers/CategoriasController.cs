using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Administrador")]
public class CategoriasController : Controller
{
    private readonly CategoriasClientService _categorias;

    public CategoriasController(CategoriasClientService categorias)
    {
        _categorias = categorias;
    }

    public async Task<IActionResult> Index()
    {
        List<Categoria>? lista = new();
        try
        {
            lista = await _categorias.GetAsync();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }

        return View(lista);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        Categoria? item = null;
        try
        {
            item = await _categorias.GetAsync(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }

        return View(item);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CrearAsync(Categoria itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _categorias.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError(
            "Nombre",
            "No ha sido posible realizar la acción. Inténtelo nuevamente"
        );
        return View(itemToCreate);
    }

    public async Task<IActionResult> EditarAsync(int id)
    {
        Categoria? itemToEdit = null;
        try
        {
            itemToEdit = await _categorias.GetAsync(id);
            if (itemToEdit == null) return NotFound();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }

        return View(itemToEdit);
    }

    [HttpPost]
    public async Task<IActionResult> EditarAsync(int id, Categoria itemToEdit)
    {
        if (id != itemToEdit.CategoriaId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _categorias.PutAsync(itemToEdit);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }

        ModelState.AddModelError(
            "Nombre",
            "No ha sido posible realizar la acción. Inténtelo nuevamente"
        );
        return View(itemToEdit);
    }

    public async Task<IActionResult> Eliminar(int id, bool? showError = false)
    {
        Categoria? itemToDelete = null;
        if (showError.GetValueOrDefault())
        {
            ViewData["ErrorMessage"] = "No ha sido posible realizar la acción. Inténtelo nuevamente.";
        }

        try
        {
            itemToDelete = await _categorias.GetAsync(id);
            if (itemToDelete == null) return NotFound();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }

        return View(itemToDelete);
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _categorias.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
        }

        return RedirectToAction(nameof(Eliminar), new { id, showError = true });
    }
}