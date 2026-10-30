namespace AnuncieCompre.Web.DTO;

public record GetCustomersResponse
{
    public List<CustomerDTO> Customers { get; set; } = [];
}