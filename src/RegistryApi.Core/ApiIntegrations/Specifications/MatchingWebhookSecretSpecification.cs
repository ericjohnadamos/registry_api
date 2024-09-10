namespace RegistryApi.Core.ApiIntegrations.Specifications;

using RegistryApi.SharedKernel;
using System;
using System.Linq.Expressions;

public class MatchingWebhookSecretSpecification(string webhookSecret) : Specification<ApiIntegration>
{
    private readonly string webhookSecret = webhookSecret;

    public override Expression<Func<ApiIntegration, bool>> ToExpression()
        => integration => integration.Md5WebhookSecret == this.webhookSecret;
}
