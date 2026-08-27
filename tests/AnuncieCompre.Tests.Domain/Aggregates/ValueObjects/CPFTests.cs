using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Tests.Domain.Aggregates.ConversationAggregate.Flows;

public class CPFTests
{

    [Theory]
    [InlineData("838.611.700-19")]
    [InlineData("098.870.870-17")]
    [InlineData("77005235509")]
    public void Create_ValidCPF_ShouldCreateCPF(string cpf)
    {
        Result<CPF> result = CPF.Create(cpf);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("VODocument criado com sucesso");
        result.Value.Should().BeOfType<CPF>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("098.870")]
    [InlineData("12345678901")]
    public void Create_InvalidCPF_ShouldNotCreateCPF(string cpf)
    {
        Result<CPF> result = CPF.Create(cpf);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("CPF inválido");
        result.Value.Should().BeNull();
    }
}