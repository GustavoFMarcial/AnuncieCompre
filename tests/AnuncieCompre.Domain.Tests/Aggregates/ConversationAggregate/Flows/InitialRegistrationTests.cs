using AnuncieCompre.Domain.Conversation.Flows;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Interfaces;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ConversationAggregate.Flows;

public class InitialRegistrationFlowTests
{

    [Fact]
    public void Build_ValidateNodes()
    {
        IReadOnlyDictionary<string, IConversationNode> initialFlow = InitialRegistrationFlow.Build();

        foreach (KeyValuePair<string, IConversationNode> node in initialFlow)
        {
            InitialValidationFlow.Validate(node.Value);

            if (node.Value.Id == "initial_ask_name")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "Gustavo F Marcial");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "Gustavo F. Marcial");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }

            if (node.Value.Id == "initial_ask_email")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "teste@gmail.com");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "teste@teste");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }

            if (node.Value.Id == "initial_ask_user_type")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "1");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "99");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }
        }
    }
}