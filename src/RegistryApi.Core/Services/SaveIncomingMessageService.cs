namespace RegistryApi.Core.Services;

using RegistryApi.Core.Interfaces;
using RegistryApi.Core.Messages;
using RegistryApi.SharedKernel;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

public class SaveIncomingMessageService : ISaveIncomingMessageService
{
    private readonly IRegistryRepository<IncomingMessage> repository;

    public SaveIncomingMessageService(IRegistryRepository<IncomingMessage> repository)
    {
        Contract.Assert(repository != null);

        this.repository = repository;
    }

    public async Task SaveIncomingMessage(
        string eventName, int customerId, string message, string contact, DateTime timestamp)
    {
        IncomingMessage incomingMessage = eventName switch
        {
            IncomingMessageEventType.OPT_OUT => IncomingMessage.CreateForOptOutEvent(customerId, contact, timestamp),
            IncomingMessageEventType.MESSAGE_SENT => IncomingMessage.CreateForMessageSentEvent(customerId, message, timestamp),
            _ => throw new ArgumentException("Invalid event name"),
        };
        await this.repository.SaveChangesAsync(incomingMessage);
    }
}
