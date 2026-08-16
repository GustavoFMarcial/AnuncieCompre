using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Enums;
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
            ValidationKind = c.ValidationKind switch
            {
                ValidationKind.Final => "Final",
                ValidationKind.Option => "Option",
                ValidationKind.Confirmation => "Confirmation",
                ValidationKind.Validation => "Validation",
                _ => "None",
            },
            ValueObjectValidator = c.ValueObjectValidator switch
            {
                ValueObjectValidator.Email => "Email",
                ValueObjectValidator.Name => "Name",
                ValueObjectValidator.Quantity => "Quantity",
                ValueObjectValidator.Product => "Product",
                ValueObjectValidator.CompanyCategory => "CompanyCategory",
                ValueObjectValidator.CPF => "CPF",
                ValueObjectValidator.CNPJ => "CNPJ",
                ValueObjectValidator.Phone => "Phone",
                ValueObjectValidator.UserType => "UserType",
                _ => "None",
            },
            Options = c.Options,
            Transitions = c.Transitions,
            IsFinal = c.IsFinal,

        }).ToList();

        return nodes;
    }

    public static CreateConversationNodeResponse ToCreateConversationNodeResponse(this ConversationNode conversationNode)
    {
        return new CreateConversationNodeResponse
        {
            Id = conversationNode.Id.ToString(),
            Message = conversationNode.Message,
            ValidationKind = conversationNode.ValidationKind switch
            {
                ValidationKind.Final => "Final",
                ValidationKind.Option => "Option",
                ValidationKind.Confirmation => "Confirmation",
                ValidationKind.Validation => "Validation",
                _ => "None",
            }
        };
    }
}