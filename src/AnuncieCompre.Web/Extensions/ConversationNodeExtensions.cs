using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Services.ValueObjectValidators;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class ConversationNodeExtensions
{
    public static List<Node> ToNodeDTO(this List<ConversationNode> conversationNodes)
    {
        List<Node> nodes = conversationNodes.Select(c => new Node
        {
            Id = c.Id,
            Message = c.Message,
            ValidationKind = c.NodeValidator switch
            {
                FinalNodeValidator => "Final",
                OptionNodeValidator => "Option",
                ConfirmationNodeValidator => "Confirmation",
                ValidationNodeValidator => "Validation",
                _ => null!,
            },
            ValueObjectValidator = c.ValueObjectValidator switch
            {
                EmailValidator => "Email",
                NameValidator => "Name",
                QuantityValidator => "Quantity",
                ProductValidator => "Product",
                CompanyCategoryValidator => "CompanyCategory",
                CpfValidator => "CPF",
                CnpjValidator => "CNPJ",
                PhoneValidator => "Phone",
                UserTypeValidator => "UserType",
                _ => "None",
            },
            Options = c.Options,
            Transitions = c.Transitions,
            IsFinal = c.IsFinal,

        }).ToList();

        return nodes;
    }
}