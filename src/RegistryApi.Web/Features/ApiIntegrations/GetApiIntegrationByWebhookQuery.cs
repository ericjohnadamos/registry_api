namespace RegistryApi.Web.Features.ApiIntegrations;

using RegistryApi.Core.ApiIntegrations;
using MediatR;

public record GetApiIntegrationByWebhookQuery(string WebhookSecret)
    : IRequest<ApiIntegration>;
