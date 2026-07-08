using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ValueObjects;

public class OptionTests
{

    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    public void Create_ValidOption_ShouldCreateOption(string options)
    {
        Result<Option> result = Option.Create(["1", "2", "3"], options);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("VOOption criado com sucesso");
        result.Value.Should().BeOfType<Option>();
    }

    [Theory]
    [InlineData("999")]
    [InlineData("")]
    [InlineData("@")]
    public void Create_InvalidOption_ShouldNotCreateOption(string options)
    {
        Result<Option> result = Option.Create(["1", "2", "3"], options);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Opção inválida, escolha novamente");
        result.Value.Should().BeNull();
    }
}