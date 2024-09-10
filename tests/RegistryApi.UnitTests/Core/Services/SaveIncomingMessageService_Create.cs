namespace RegistryApi.UnitTests.Core.Services;

using RegistryApi.Core.Messages;
using RegistryApi.Core.Services;
using RegistryApi.SharedKernel;
using FluentAssertions;
using Moq;
using Xunit;

public class SaveIncomingMessageService_Create
{
    private readonly Mock<IRegistryRepository<IncomingMessage>> mockRepository = new();
    private readonly SaveIncomingMessageService service;

    public SaveIncomingMessageService_Create()
    {
        this.service = new SaveIncomingMessageService(this.mockRepository.Object);
    }

    [Fact]
    public void SaveIncomingMessage_WithOptOutEvent_ShouldSucceedWithoutExceptions()
    {
        Func<Task> persistence = async () => await this.service
            .SaveIncomingMessage(IncomingMessageEventType.OPT_OUT, 33, string.Empty, "041234", DateTime.UtcNow);
        persistence.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public void SaveIncomingMessage_WithMessageSentEvent_ShouldSucceedWithoutExceptions()
    {
        Func<Task> persistence = async () => await this.service
            .SaveIncomingMessage(IncomingMessageEventType.MESSAGE_SENT, 33, "Message", "", DateTime.UtcNow);
        persistence.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public void SaveIncomingMessageService_WithUnrecognisedEvent_ShouldThrowArgumentException()
    {
        Func<Task> persistence = async () => await this.service
            .SaveIncomingMessage("UnrecognisedEvent", 33, "Message", "041234", DateTime.UtcNow);
        persistence.Should().ThrowAsync<ArgumentException>();
    }
}
