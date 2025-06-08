using frontendnet.Models;

public class PedidosClientService(HttpClient client)
{
    // Obtener el carrito del usuario
    public async Task<Pedido?> ObtenerCarritoAsync(string email)
    {
        return await client.GetFromJsonAsync<Pedido>($"api/usuarios/{email}/carrito");
    }

    // Obtener los items del carrito
    public async Task<List<ItemPedido>?> ObtenerItemsCarritoAsync(string email)
    {
        return await client.GetFromJsonAsync<List<ItemPedido>>($"api/usuarios/{email}/carrito/items");
    }

    // Agregar un producto al carrito
    public async Task AgregarAlCarritoAsync(string email, int productoId, int cantidad, decimal precioUnitario)
    {
        var data = new { productoid = productoId, cantidad, precioUnitario };
        await client.PostAsJsonAsync($"api/usuarios/{email}/carrito/items", data);
    }

    // Comprar ahora (sin carrito)
    public async Task ComprarAhoraAsync(string email, int productoId, int cantidad, string direccionEnvio, string metodoPago)
    {
        var producto = await client.GetFromJsonAsync<Producto>($"api/productos/{productoId}");
        var precioUnitario = producto?.Precio ?? 0;

        var data = new
        {
            item = new { productoid = productoId, cantidad, precioUnitario },
            direccionEnvio,
            metodoPago
        };
        await client.PostAsJsonAsync($"api/usuarios/{email}/pedidos", data);
    }

    // Eliminar un item del carrito
    public async Task EliminarDelCarritoAsync(string email, int itemId)
    {
        await client.DeleteAsync($"api/usuarios/{email}/carrito/items/{itemId}");
    }

    // Confirmar carrito (comprar todo)
    public async Task ConfirmarCarritoAsync(string email)
    {
        await client.PostAsync($"api/usuarios/{email}/carrito/confirmar", null);
    }

    public async Task ModificarCantidadCarritoAsync(string email, int itemId, int cantidad)
    {
        var data = new { cantidad };
        await client.PutAsJsonAsync($"api/usuarios/{email}/carrito/items/{itemId}", data);
    }

    // Obtener todos los pedidos 
    public async Task<List<Pedido>?> ObtenerPedidosAsync()
    {
        return await client.GetFromJsonAsync<List<Pedido>>($"api/pedidos");
    }

    // Obtener pedidos del usuario
    public async Task<List<Pedido>?> ObtenerPedidosPorUsuarioAsync(string email)
    {
        return await client.GetFromJsonAsync<List<Pedido>>($"api/usuarios/{email}/pedidos");
    }


}