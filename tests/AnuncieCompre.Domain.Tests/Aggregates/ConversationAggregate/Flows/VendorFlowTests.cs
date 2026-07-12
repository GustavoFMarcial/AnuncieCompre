using AnuncieCompre.Domain.Conversation.Flows;
using AnuncieCompre.Domain.Interfaces;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ConversationAggregate.Flows;

public class VendowFlowTests
{
    [Fact]
    public void Build_ValidateNodes()
    {
        IReadOnlyDictionary<string, IConversationNode> initialFlow = InitialRegistrationFlow.Build();
        IReadOnlyDictionary<string, IConversationNode> vendorFlow = VendorFlow.Build(initialFlow);

        foreach (KeyValuePair<string, IConversationNode> node in vendorFlow)
        {
            InitialValidationFlow.Validate(node.Value);

            if (node.Value.Id == "vendor_ask_company_category")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "1");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "838.611");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }

            if (node.Value.Id == "vendor_ask_company_name")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "Jonas da Bola Alada LTDA");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "as");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }

            if (node.Value.Id == "vendor_ask_cnpj")
            {
                var result1 = node.Value.NodeValidator.Validate(node.Value, "04.100.180/0001-34");
                var result2 = node.Value.NodeValidator.Validate(node.Value, "12.345.678/9012-34");

                result1.IsSuccess.Should().BeTrue();
                result2.IsSuccess.Should().BeFalse();
            }
        }
    }
}