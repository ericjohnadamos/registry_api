namespace RegistryApi.Web.Features.ApiIntegrations;

using RegistryApi.Core.ApiIntegrations;
using RegistryApi.Core.ApiIntegrations.Specifications;
using RegistryApi.Infrastructure.Data;
using RegistryApi.SharedKernel.Exceptions;
using MediatR;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

public class GetApiIntegrationByWebhookQueryHandler
    : IRequestHandler<GetApiIntegrationByWebhookQuery, ApiIntegration>
{
    private readonly IUserRepository<ApiIntegration> repository;

    public GetApiIntegrationByWebhookQueryHandler(IUserRepository<ApiIntegration> repository)
    {
        Contract.Assert(repository != null);

        this.repository = repository;
    }

    public async Task<ApiIntegration> Handle(
        GetApiIntegrationByWebhookQuery request, CancellationToken cancellationToken)
    {
        var specification = new MatchingWebhookSecretSpecification(request.WebhookSecret);
        var integration = await this.repository.FirstOrDefaultAsync(specification)
            ?? throw new NotFoundException($"When trying to find api integration with the web hook secret of '{request.WebhookSecret}', nothing came up. Please ensure that you are passing the correct web hook secret and in proper hashing.");
        return integration;
    }
}

