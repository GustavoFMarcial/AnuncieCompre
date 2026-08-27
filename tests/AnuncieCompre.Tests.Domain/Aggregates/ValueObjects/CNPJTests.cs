using System.IO.Pipelines;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Tests.Domain.Aggregates.ConversationAggregate.Flows;

public class CNPJTests
{

    [Theory]
    [InlineData("24823317000196")]
    [InlineData("04.100.180/0001-34")]
    [InlineData("32.773.509/0001-63")]
    public void Create_ValidCNPJ_ShouldCreateCNPJ(string cnpj)
    {
        Result<CNPJ> result = CNPJ.Create(cnpj);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("VOCNPJ criado com sucesso");
        result.Value.Should().BeOfType<CNPJ>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("111111111")]
    [InlineData("12.345.678/9012-34")]
    public void Create_InvalidCNPJ_ShouldNotCreateCNPJ(string cnpj)
    {
        Result<CNPJ> result = CNPJ.Create(cnpj);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("CNPJ inválido");
        result.Value.Should().BeNull();
    }
}