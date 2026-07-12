using AnuncieCompre.Domain.Conversation.Flows;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Interfaces;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ConversationAggregate.Flows;

public class CustomerFlowTests
{

    [Fact]
    public void Build_ValidateNodes()
    {
        IReadOnlyDictionary<string, IConversationNode> initialFlow = InitialRegistrationFlow.Build();
        IReadOnlyDictionary<string, IConversationNode> customerFlow = CustomerFlow.Build(initialFlow);

        foreach (KeyValuePair<string, IConversationNode> node in customerFlow)
        {
            InitialValidationFlow.Validate(node.Value);

            if (node.Value.Id == "customer_ask_cpf")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "838.611.700-19");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "838.611");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }

            if (node.Value.Id == "customer_ask_company_category")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "1");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "abc");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }

            if (node.Value.Id == "customer_ask_product")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "disco de freio");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }

            if (node.Value.Id == "customer_ask_quantity")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "1 kg");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }
        }
    }
}