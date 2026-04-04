using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Services;

public class ValidateMessage
{
    public static NodeResult Handle(ConversationNode awaitingResponseNode, ConversationNode ActiveNode, string message)
    {
        if (awaitingResponseNode.Options is not null)
        {
            foreach (string o in awaitingResponseNode.Options)
            {
                if (message == o) return new NodeResult(true, ActiveNode.Message, message);
            }
            return new NodeResult(false, "Opção inválida, escolha novamente.");
        }
        else
        {
            IResultUntyped result = awaitingResponseNode.Validate(message);
            if (result.IsSuccess) return new NodeResult(result.IsSuccess, ActiveNode.Message, "next", (ValueObject?)result.Value);
            else return new NodeResult(result.IsSuccess, result.Message);
        }
    }
}