using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Tests.Domain.Aggregates.ConversationAggregate.Flows;

public class QuantityTests
{
    
    [Theory]
    [InlineData("1kg")]
    [InlineData("0,1")]
    [InlineData("0.99")]
    [InlineData("5 ")]
    [InlineData("1 arroba")]
    [InlineData("1@")]
    [InlineData("1 m³")]
    public void Create_ValidQuantity_ShouldCreateQuantity(string quantity)
    {
        Result<Quantity> result = Quantity.Create(quantity);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("VOQuantity criado com sucesso");
        result.Value.Should().BeOfType<Quantity>();
    }

    [Theory]
    [InlineData("0")]
    [InlineData("0,0kg")]
    [InlineData("-9999")]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("0 kg")]
    [InlineData("0,0")]
    [InlineData("0.0")]
    [InlineData("0.0 kg")]
    public void Create_InvalidQuantity_ShouldNotCreateQuantity(string quantity)
    {
        Result<Quantity> result = Quantity.Create(quantity);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Quantidade inválida");
        result.Value.Should().BeNull();
    }
}