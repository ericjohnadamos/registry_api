using System.Text.Json.Serialization;

namespace RegistryApi.Web.Endpoints.Users;

public class CreateUserRequest
{
    public const string Route = "api/user";

    [JsonPropertyName("username")]
    public string Username { get; set; } = default!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = default!;

    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }
}
