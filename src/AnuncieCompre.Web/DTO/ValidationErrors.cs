namespace AnuncieCompre.Web.DTO;
public record NodeValidationErrors(bool _success, List<string> _errors)
{
    public bool Success { get; set; } = _success;
    public List<string> Errors {get; set; } = _errors;   
}