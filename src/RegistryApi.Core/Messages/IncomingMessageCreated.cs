namespace RegistryApi.Core.Messages;

using RegistryApi.SharedKernel;

public class IncomingMessageCreated : DomainEventBase
{
    public IncomingMessageCreated(IncomingMessage message)
        => this.IncomingMessage = message;

    public IncomingMessage IncomingMessage { get; }
}
