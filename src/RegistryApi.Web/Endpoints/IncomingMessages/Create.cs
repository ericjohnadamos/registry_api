namespace RegistryApi.Web.Endpoints.IncomingMessages;

using Ardalis.ApiEndpoints;
using RegistryApi.Web.Attributes;
using RegistryApi.Web.Features.IncomingMessages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

[ApiVersion("1.0")]
public class Create : EndpointBaseAsync
    .WithRequest<object>
    .WithActionResult
{
    private readonly IMediator mediator;
    private readonly ILogger<Create> logger;

    public Create(IMediator mediator, ILogger<Create> logger)
    {
        Contract.Assert(mediator != null);
        Contract.Assert(logger != null);

        this.mediator = mediator;
        this.logger = logger;
    }

    /// <summary>
    /// Method that persists the incoming messages in the registry.
    /// </summary>
    /// <remarks>
    /// The request contains web hook secret that is needed to verify the integrity of the request.
    /// All of the event data that is sent through a single standard HTTP POST field are formatted as JSON.
    /// This must respond with 200 level response code. Any response outside of the 2xx range, including
    /// 3xx redirects, will indicate that we did not receive the web hook data successfully.
    /// </remarks>
    /// <param name="request">Request parameter in the form of <see cref="CreateIncomingMessageRequest"/>.</param>
    /// <param name="cancellationToken">Propagates information that operations should be canceled.</param>
    /// <returns></returns>
    [SlicktextSignatureAuthorization]
    [HttpPost(CreateIncomingMessageRequest.Route)]
    [SwaggerOperation(
        Summary = "Creates a new incoming message record",
        Description = "Creates a new incoming message record in the registry",
        OperationId = "IncomingMessages.Create",
        Tags = ["IncomingMessages"])]
    public override async Task<ActionResult> HandleAsync(object json, CancellationToken cancellationToken = new())
    {
        this.logger.LogInformation($"Logging information from IncomingMessages.Create request: {json}");
        
        // With the use of slick signature authorisation attribute, guaranteed that it has this integration customer id
        var customerId = Int16.Parse(
            Request.Headers[SlicktextSignatureAuthorizationAttribute.INTEGRATION_CUSTOMER_ID].ToString());

        var request = JsonConvert.DeserializeObject<CreateIncomingMessageRequest>(json?.ToString() ?? "");
        if (request == null)
            return BadRequest($"Cannot parse json object: {json}");

        var timestamp = DateTime.ParseExact(request.Timestamp, "yyyy-MM-dd HH:mm:ss", null);
        var command = new SaveIncomingMessageCommand(
            request.Event,
            customerId,
            request.Message?.Body ?? "",
            request.Subscriber?.Number ?? "",
            timestamp);
        await this.mediator.Send(command);

        return Ok();
    }
}
