using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ValueObjects;

public class NameTests
{
    
    [Theory]
    [InlineData("Jonas Alado Nunes")]
    [InlineData("Plinio da Silva Pinto")]
    [InlineData("Scarlett Ingrid Johansson")]
    public void Create_ValidName_ShouldCreateName(string name)
    {
        Result<Name> result = Name.Create(name);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("VOName criado com sucesso");
        result.Value.Should().BeOfType<Name>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Jonas Alado Nunes Plinio da Silva Pinto Scarlett Ingrid Johansson")]
    [InlineData("Scarlett I. Johansson")]
    public void Create_InvalidName_ShouldNotCreateName(string name)
    {
        Result<Name> result = Name.Create(name);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}