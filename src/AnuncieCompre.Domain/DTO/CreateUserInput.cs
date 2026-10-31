namespace AnuncieCompre.Domain.DTO;

public record CreateUserInput
{
    public string Name { get; set; } = default!;
    public string Cpf { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;
}