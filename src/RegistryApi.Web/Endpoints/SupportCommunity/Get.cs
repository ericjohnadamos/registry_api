namespace RegistryApi.Web.Endpoints.SupportCommunity;

using RegistryApi.Web.Features.Customers;
using RegistryApi.Web.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

// Notes: Registry endpoint base needs refactoring.
// I suggest using endpoint base async and get rid of the mediator as requirement since it is not
// always necessary.

[ApiVersion("1.0")]
public class Get : RegistryEndpointBase
{
    public Get(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        : base(mediator, httpContextAccessor)
    {
    }

    [Authorize]
    [HttpGet("/api/customer/support-community")]
    [HttpGet("/api/customer/supportcommunity")]
    [SwaggerOperation(
        Summary = "Get the list of customer stays",
        Description = "Get the list of customer stays",
        OperationId = "SupportCommunity.Get",
        Tags = new[] { "SupportCommunity" })]
    public async Task<GetCustomerStaysResponse> GetCustomerStays()
    {
        return await this.Mediator.Send(new GetCustomerStaysQuery(UserInfo, User));
    }
}
