namespace RegistryApi.Web.Endpoints.Users;

using Ardalis.ApiEndpoints;
using RegistryApi.Web.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

[ApiVersion("1.0")]
public class Create : EndpointBaseAsync
    .WithRequest<CreateUserRequest>
    .WithActionResult
{
    private readonly IMediator mediator;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IConfiguration configuration;
    private readonly ILogger<Create> logger;

    public Create(
        IMediator mediator,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        ILogger<Create> logger)
    {
        Contract.Assert(mediator != null);
        Contract.Assert(httpContextAccessor != null);
        Contract.Assert(configuration != null);
        Contract.Assert(logger != null);

        this.mediator = mediator;
        this.httpContextAccessor = httpContextAccessor;
        this.configuration = configuration;
        this.logger = logger;
    }

    [AllowAnonymous]
    [HttpPost(CreateUserRequest.Route)]
    [SwaggerOperation(
        Summary = "Creates a new user record",
        Description = "Creates a new user record",
        OperationId = "Users.Create",
        Tags = ["Users"])]
    public override async Task<ActionResult> HandleAsync(
        CreateUserRequest model, CancellationToken cancellationToken = default)
    {
        var authHeader = AuthenticationHeaderValue.Parse(
            this.httpContextAccessor.HttpContext!.Request.Headers["Authorization"]);
        var key = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter!));
        if (key != this.configuration["systemAdminKey"])
        {
            return new UnauthorizedResult();
        }

        var errorList = new List<string>();
        if (string.IsNullOrWhiteSpace(model.Username))
            errorList.Add("Username is required.");
        if (string.IsNullOrWhiteSpace(model.Password))
            errorList.Add("Password is required.");
        if (model.CustomerId <= 0)
            errorList.Add("Please provide a valid customer id.");
        if (errorList.Any())
        {
            this.logger.LogError($"User failed to create. Data: {string.Join(", ", errorList)}");
            return new BadRequestObjectResult(errorList);
        }

        var createCommand = new CreateUserCommand(model.Username, model.Password, model.CustomerId);
        var response = await this.mediator.Send(createCommand);

        if (!response.WasSuccessful)
        {
            throw response.Exception;
        }

        this.logger.LogInformation($"User with username: {model.Username} was created successfully.");

        return new NoContentResult();
    }
}
