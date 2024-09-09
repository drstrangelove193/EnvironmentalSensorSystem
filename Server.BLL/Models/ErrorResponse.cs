using System.Text.Json;

namespace Server.BLL.Models;

public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}
