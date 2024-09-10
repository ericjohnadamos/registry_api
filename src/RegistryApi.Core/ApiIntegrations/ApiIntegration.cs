namespace RegistryApi.Core.ApiIntegrations;

using RegistryApi.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

public class ApiIntegration : IEntity
{
    private ApiIntegration()
    {
        Id = default!;
        CustomerId = default!;
        Md5WebhookSecret = default!;
        Username = default!;
        Password = default!;
    }

    public int Id { get; private set; }

    public int CustomerId { get; private set; }

    public string Username { get; private set; }

    public string Password { get; private set; }

    [Column("md5_webhooksecret")]
    public string? Md5WebhookSecret { get; private set; }
}
