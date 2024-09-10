namespace RegistryApi.Core.Messages;

using RegistryApi.SharedKernel;
using System;

public class IncomingMessage : DomainEntityBase
{
    private IncomingMessage()
    {
    }

    private IncomingMessage(
        string eventName, int customerId, string? message, string? contactNumber, DateTime timestamp)
    {
        this.Event = eventName;
        this.CustomerId = customerId;
        this.Message = message;
        this.ContactNumber = contactNumber;
        this.EventDate = timestamp;
        this.CreationDate = DateTime.Now;

        this.Raise(new IncomingMessageCreated(this));
    }

    public int CustomerId { get; private set; }

    public string? Event { get; private set; }

    public string? Message { get; private set; }

    public string? ContactNumber { get; private set; }

    public DateTime? EventDate { get; private set; }

    public DateTime? CreationDate { get; private set; }

    public static IncomingMessage CreateForMessageSentEvent(int customerId, string message, DateTime timestamp)
        => new(IncomingMessageEventType.MESSAGE_SENT, customerId, message, null, timestamp);

    public static IncomingMessage CreateForOptOutEvent(int customerId, string contactNumber, DateTime timestamp)
        => new(IncomingMessageEventType.OPT_OUT, customerId, null, contactNumber, timestamp);
}
