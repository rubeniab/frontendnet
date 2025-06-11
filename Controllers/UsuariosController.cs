using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

[Authorize(Roles = "Administrador")]
public class UsuariosController(UsuariosClientService usuarios, RolesClientService roles) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Usuario>? lista = [];
        try
        {
            lista = await usuarios.GetAsync();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(lista);
    }

    public async Task<IActionResult> Detalle(string id)
    {
        Usuario? item = null;
        try
        {
            item = await usuarios.GetAsync(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(item);
    }

    public async Task<IActionResult> Crear()
    {
        await RolesDropDownListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CrearAsync(UsuarioPwd itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await usuarios.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }
        ModelState.AddModelError("Email", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        await RolesDropDownListAsync();
        return View(itemToCreate);
    }

    [HttpGet("[controller]/[action]/{email?}")]
    public async Task<IActionResult> EditarAsync(string email)
    {
        Usuario? itemToEdit = null;
        try
        {
            itemToEdit = await usuarios.GetAsync(email);
            ViewBag.PuedeEditar = (User.Identity?.Name == email);

              await RolesDropDownListAsync(itemToEdit?.Rol);
            return View(itemToEdit);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
            else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["ErrorMessage"] = "No se ha encontrado un usuario asociado.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error inesperado en el servidor.";
                return RedirectToAction("Index");
            }
        }
    }


[HttpPost("[controller]/[action]/{email?}")]
public async Task<IActionResult> EditarAsync(Usuario itemToEdit)
{
    if (ModelState.IsValid)
    {
        try
        {
            await usuarios.PutAsync(itemToEdit);
            TempData["SuccessMessage"] = "Usuario modificado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            await RolesDropDownListAsync(itemToEdit.Rol);
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
                else if (ex.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    TempData["ErrorMessage"] = "Correo inválido.";
                else if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    TempData["ErrorMessage"] = "No se ha encontrado un usuario asociado.";
                else if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    TempData["ErrorMessage"] = "El nombre del usuario es obligatorio y debe ser una cadena con al menos un carácter.";
                else
                    TempData["ErrorMessage"] = "Ha ocurrido un error inesperado en el servidor.";

            return View(itemToEdit);
        }
    }

    TempData["ErrorMessage"] = "No ha sido posible realizar la acción. Inténtelo nuevamente.";
    return View(itemToEdit);
}

    public async Task<IActionResult> Eliminar(string id, bool? showError = false)
    {
        Usuario? itemToDelete = null;
        try
        {
            itemToDelete = await usuarios.GetAsync(id);
            if (itemToDelete == null) return NotFound();

            if (showError.GetValueOrDefault())
                ViewData["ErrorMessage"] = "No ha sido posible realizar la acción. Inténtelo nuevamente.";
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        ViewBag.PuedeEditar = !(User.Identity?.Name == id);
        return View(itemToDelete);
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(string id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await usuarios.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth");
            }
        }

        return RedirectToAction(nameof(Eliminar), new { id, showError = true });
    }

    private async Task RolesDropDownListAsync(object? rolSeleccionado = null)
    {
        var listado = await roles.GetAsync();
        ViewBag.Rol = new SelectList(listado, "Nombre", "Nombre", rolSeleccionado);
    }

}
