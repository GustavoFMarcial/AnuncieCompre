using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record CreateCompanyRequest
{
    [Required(ErrorMessage = "Nome da empresa é obrigatório")]
    public string CompanyName { get; set; } = default!;
    [Required(ErrorMessage = "Cnpj é obrigatório")]
    public string Cnpj { get; set; } = default!;
    [Required(ErrorMessage = "Email da empresa é obrigatório")]
    [EmailAddress]
    public string Email { get; set; } = default!;
    [Required(ErrorMessage = "Telefone da empresa é obrigatório")]
    public string Phone { get; set; } = default!;
    [Required(ErrorMessage = "Cpf do usuário que vai user o login admin é obrigatório")]
    public string Cpf { get; set; } = default!;
    [Required(ErrorMessage = "Login é obrigatório")]
    public string Login { get; set; } = default!;
    [Required(ErrorMessage = "Senha é obrigatória")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}