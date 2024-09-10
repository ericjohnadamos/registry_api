namespace RegistryApi.UnitTests.Core.Messages;

using RegistryApi.Core.Messages;
using FluentAssertions;
using Xunit;

public class Message_Create
{
    [Fact]
    public void CreateIncomingMessage_ForOptOutEvent_ShouldSucceed()
    {
        var customerId = 33;
        var contactNumber = @"0412341234";
        var timestamp = DateTime.UtcNow;

        var incomingMessage = IncomingMessage.CreateForOptOutEvent(customerId, contactNumber, timestamp);

        incomingMessage.Should().NotBeNull();
        incomingMessage.Event.Should().Be(IncomingMessageEventType.OPT_OUT);
        incomingMessage.EventDate.Should().Be(timestamp);
        incomingMessage.CustomerId.Should().Be(customerId);
        incomingMessage.ContactNumber.Should().Be(contactNumber);
        incomingMessage.Message.Should().BeNull();

        incomingMessage.DomainEvents.Should().HaveCount(1);
    }

    [Fact]
    public void CreateIncomingMessage_ForMessageSentEvent_ShouldSucceed()
    {
        var customerId = 33;
        var message = @"Random Message";
        var timestamp = DateTime.UtcNow;

        var incomingMessage = IncomingMessage.CreateForMessageSentEvent(customerId, message, timestamp);

        incomingMessage.Should().NotBeNull();
        incomingMessage.Event.Should().Be(IncomingMessageEventType.MESSAGE_SENT);
        incomingMessage.EventDate.Should().Be(timestamp);
        incomingMessage.CustomerId.Should().Be(customerId);
        incomingMessage.Message.Should().Be(message);
        incomingMessage.ContactNumber.Should().BeNull();

        incomingMessage.DomainEvents.Should().HaveCount(1);
    }
}
