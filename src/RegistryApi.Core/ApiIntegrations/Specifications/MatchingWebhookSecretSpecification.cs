namespace RegistryApi.Core.ApiIntegrations.Specifications;

using RegistryApi.SharedKernel;
using System;
using System.Linq.Expressions;

public class MatchingWebhookSecretSpecification : Specification<ApiIntegration>
{
    private readonly string webhookSecret;

    public MatchingWebhookSecretSpecification(string webhookSecret)
    {
        this.webhookSecret = webhookSecret;
    }

    public override Expression<Func<ApiIntegration, bool>> ToExpression()
        => integration => integration.Md5WebhookSecret == this.webhookSecret;
}
