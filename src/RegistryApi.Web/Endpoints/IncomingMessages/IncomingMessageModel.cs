namespace RegistryApi.Web.Endpoints.IncomingMessages;
public class IncomingMessageModel
{
    public string Event { get; set; } = default!;
    public string Timestamp { get; set; } = default!;
    public int? AttemptNumber { get; set; }
    public Subscriber? Subscriber { get; set; }
    public Message? Message { get; set; }
}

public class Message
{
    public string? MessageId { get; set; }
    public string? DateScheduled { get; set; }
    public string? DateSent { get; set; }
    public string? DateCreated { get; set; }
    public string? TextwordId { get; set; }
    public bool SegmentId { get; set; }
    public bool SubscriberId { get; set; }
    public string? CampaignName { get; set; }
    public string? Body { get; set; }
    public string? MediaUrl { get; set; }
    public int SubscriberCount { get; set; }
}

public class Subscriber
{
    public string? SubscriberId { get; set; }
    public string? TextwordId { get; set; }
    public string? Number { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Birthdate { get; set; }
    public string? Email { get; set; }
    public string? DateSubscribed { get; set; }
    public string? DateOptedOut { get; set; }
    public string? OptInSource { get; set; }
    public string? FavoriteFood { get; set; }
}
