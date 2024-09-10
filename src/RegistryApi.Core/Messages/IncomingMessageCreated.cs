namespace RegistryApi.Core.Messages;

using RegistryApi.SharedKernel;

public class IncomingMessageCreated(IncomingMessage message) : DomainEventBase
{
    public IncomingMessage IncomingMessage { get; } = message;
}
