namespace ResilientWebhook.Core;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAtUtc { get; set; }
    public int RetryCount { get; set; }
    public string? LastError { get; set; }
}

public class WebhookSubscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string DestinationUrl { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class WebhookDeliveryAttempt
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MessageId { get; set; }
    public string DestinationUrl { get; set; } = string.Empty;
    public int AttemptNumber { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime AttemptedAtUtc { get; set; } = DateTime.UtcNow;
    public string? ResponseSnippet { get; set; }
}
