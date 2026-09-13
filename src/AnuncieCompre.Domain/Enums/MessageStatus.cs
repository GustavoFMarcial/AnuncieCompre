namespace AnuncieCompre.Domain.Enums;

public enum MessageStatus
{
    Queued,
    Sending,
    Sent,
    Delivered,
    Read,
    Failed,
    Undelivered,
}