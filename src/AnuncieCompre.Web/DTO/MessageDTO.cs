namespace AnuncieCompre.Web.DTO;

public record MessageDTO
{
    public string Text { get; set; } = default!;
    public string SenderType { get; set; } = default!;
    public string Direction { get; set; } = default!;
}