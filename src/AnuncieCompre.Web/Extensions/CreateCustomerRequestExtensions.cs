using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class CreateCustomerRequestExtensions
{
    public static CreateCustomerInput ToCreateCustomerInput(this CreateCustomerRequest request)
    {
        return new CreateCustomerInput
        {
            Phone = request.Phone,
            Name = request.Name,
            Email = request.Email,
        };
    }
}