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
        IncomingMessage incomingMessage;

        // Note: Refactor this in the future by using runtime parsing
        switch (eventName)
        {
            case IncomingMessageEventType.OPT_OUT:
                incomingMessage = IncomingMessage.CreateForOptOutEvent(customerId, contact, timestamp);
                break;
            case IncomingMessageEventType.MESSAGE_SENT:
                incomingMessage = IncomingMessage.CreateForMessageSentEvent(customerId, message, timestamp);
                break;
            default:
                throw new ArgumentException("Invalid event name");
        }

        await this.repository.SaveChangesAsync(incomingMessage);
    }
}
