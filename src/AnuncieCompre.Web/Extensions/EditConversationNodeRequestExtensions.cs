using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class EditConversationNodeRequestExtensions
{
    public static EditConversationNodeInput ToEditConversationNodeInput(this EditConversationNodeRequest request)
    {
        return new EditConversationNodeInput
        {
            Message = request.Message,
            ValidationKind = request.ValidationKind switch
            {
                "Final" => ValidationKind.Final,
                "Option" => ValidationKind.Option,
                "Confirmation" => ValidationKind.Confirmation,
                "Validation" => ValidationKind.Validation,
                "None" => ValidationKind.None,
                _ => ValidationKind.None,
            },
            ValueObjectValidator = request.ValueObjectValidator switch
            {
                "Email" => ValueObjectValidator.Email,
                "Name" => ValueObjectValidator.Name,
                "Quantity" => ValueObjectValidator.Quantity,
                "Product" => ValueObjectValidator.Product,
                "CompanyCategory" => ValueObjectValidator.CompanyCategory,
                "CPF" => ValueObjectValidator.CPF,
                "CNPJ" => ValueObjectValidator.CNPJ,
                "Phone" => ValueObjectValidator.Phone,
                "UserType" => ValueObjectValidator.UserType,
                "None" => ValueObjectValidator.None,
                _ => ValueObjectValidator.None,
            },
            Options = request.Options,
            IsFinal = request.IsFinal,
        };
    }
}