namespace RegistryApi.Web.Features.IncomingMessages;

using RegistryApi.Core.Interfaces;
using RegistryApi.SharedKernel;
using MediatR;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

public class SaveIncomingMessageCommandHandler
     : IRequestHandler<SaveIncomingMessageCommand, Response<Unit>>
{
    private readonly ISaveIncomingMessageService service;

    public SaveIncomingMessageCommandHandler(ISaveIncomingMessageService service)
    {
        Contract.Assert(service != null);
        this.service = service;
    }

    public async Task<Response<Unit>> Handle(
        SaveIncomingMessageCommand request, CancellationToken cancellationToken)
    {
        await this.service.SaveIncomingMessage(
            request.Event, request.CustomerId, request.Message, request.Contact, request.Timestamp);
        return await Task.FromResult(new SuccessResponse<Unit>(Unit.Value));
    }
}
