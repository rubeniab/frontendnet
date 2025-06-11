namespace frontendnet.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public int Count { get; set; }
    public List<T> Data { get; set; } = [];
}
