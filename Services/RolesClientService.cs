using frontendnet.Models;

namespace frontendnet.Services;

public class RolesClientService(HttpClient client)
{
    public async Task<List<Rol>> GetAsync()
    {
        var response = await client.GetFromJsonAsync<ApiResponse<Rol>>("api/roles");
        return response?.Data ?? [];
    }
}