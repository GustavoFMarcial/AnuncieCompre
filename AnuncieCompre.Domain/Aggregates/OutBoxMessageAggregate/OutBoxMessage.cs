namespace AnuncieCompre.Domain.Aggregates.OutOfBoxAggregate;

public class OutboxMessage : BaseEntity
{
    public string EventType { get; set; } = default!;
    public string PayloadJson { get; set; } = default!;
    public bool IsProcessed { get; set; } = false;
}