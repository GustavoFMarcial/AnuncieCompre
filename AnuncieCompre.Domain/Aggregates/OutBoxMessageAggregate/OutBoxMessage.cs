namespace AnuncieCompre.Domain.Aggregates.OutOfBoxAggregate;

public class OutBoxMessage : BaseEntity
{
    // public Type EventType { get; set; } = default!;
    public string PayloadJson { get; set; } = default!;
}