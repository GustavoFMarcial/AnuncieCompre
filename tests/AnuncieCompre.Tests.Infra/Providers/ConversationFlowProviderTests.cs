using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Infra.Providers;
using FluentAssertions;
using Moq;

namespace AnuncieCompre.Infra.Tests.Providers;

public class ConversationFlowProviderTests
{

    [Theory]
    [InlineData("start")]
    [InlineData("ask_another_order")]
    [InlineData("finish")]
    public void GetById_ValidId_ShouldReturnConversationNode(string id)
    {
        ConversationFlowProvider conversationFlowProvider = new ConversationFlowProvider();
        IConversationNode result = conversationFlowProvider.GetById(id);

        result.Id.Should().Be(id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("assdadads")]
    [InlineData("da sdas  asd")]
    public void GetById_InvalidId_ShouldReturnConversationNode(string id)
    {
        ConversationFlowProvider conversationFlowProvider = new ConversationFlowProvider();
        Action result = () => conversationFlowProvider.GetById(id);

        result.Should().Throw<KeyNotFoundException>();
    }
}