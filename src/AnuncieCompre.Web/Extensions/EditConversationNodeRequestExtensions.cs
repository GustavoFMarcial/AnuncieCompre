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
            ValidationKind = request.ValidationKind.ToLower() switch
            {
                "final" => ValidationKind.Final,
                "option" => ValidationKind.Option,
                "confirmation" => ValidationKind.Confirmation,
                "validation" => ValidationKind.Validation,
                "none" => ValidationKind.None,
                _ => ValidationKind.None,
            },
            ValueObjectValidator = request.ValueObjectValidator.ToLower() switch
            {
                "email" => ValueObjectValidator.Email,
                "name" => ValueObjectValidator.Name,
                "quantity" => ValueObjectValidator.Quantity,
                "product" => ValueObjectValidator.Product,
                "companycategory" => ValueObjectValidator.CompanyCategory,
                "cpf" => ValueObjectValidator.CPF,
                "cnpj" => ValueObjectValidator.CNPJ,
                "phone" => ValueObjectValidator.Phone,
                "usertype" => ValueObjectValidator.UserType,
                "none" => ValueObjectValidator.None,
                _ => ValueObjectValidator.None,
            },
            Options = request.Options,
            IsFinal = request.IsFinal,
        };
    }
}