using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Interfaces;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ConversationAggregate.Flows;

public class InitialValidationFlow
{
    public static void Validate(IConversationNode node)
    {
        node.Id.Should().NotBeNullOrWhiteSpace();
        node.Message.Should().NotBeNullOrWhiteSpace();
        node.NodeValidator.Should().NotBeNull();

        if (node.Id != "ask_user_type")
        {
            node.Transitions.Count.Should().BeGreaterThan(0);
        }

        if (node is OptionNode || node is ConfirmationNode || node is OptionValidationNode)
        {
            node.Transitions.Count.Should().BeGreaterThan(1);
        }

        if (node is not OptionNode && node is not ConfirmationNode && node is not OptionValidationNode)
        {
            node.Transitions.Count.Should().Be(1);
        }

        if (node is ConfirmationNode)
        {
            node.NodeValidator.Should().BeOfType<ConfirmationNodeValidator>();
        }

        if (node is FinalNode)
        {
            node.NodeValidator.Should().BeOfType<FinalNodeValidator>();
        }

        if (node is OptionNode)
        {
            node.NodeValidator.Should().BeOfType<OptionNodeValidator>();
        }

        if (node is OptionValidationNode)
        {
            node.NodeValidator.Should().BeOfType<OptionValidationNodeValidator>();
        }

        if (node is ValidationNode)
        {
            node.NodeValidator.Should().BeOfType<ValidationNodeValidator>();
        }
    }
}