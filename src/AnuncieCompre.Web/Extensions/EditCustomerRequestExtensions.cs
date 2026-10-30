using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class EditCustomerRequestExtensions
{
    public static EditCustomerInput ToEditCustomerInput(this EditCustomerRequest request)
    {
        return new EditCustomerInput
        {
            Name = request.Name,
            Email = request.Email
        };
    }
}