using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ValueObjects;

public class EmailTests
{
    
    [Theory]
    [InlineData("teste@gmail.com")]
    [InlineData("teste@teste.com")]
    [InlineData("anunciecompre123@outlook.com.br")]
    public void Create_ValidEmail_ShouldCreateEmail(string email)
    {
        Result<Email> result = Email.Create(email);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("VOEmail criado com sucesso");
        result.Value.Should().BeOfType<Email>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("teste@teste")]
    [InlineData("anunciecompre123outlook.com.br")]
    public void Create_InvalidEmail_ShouldNotCreateEmail(string email)
    {
        Result<Email> result = Email.Create(email);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Email inválido");
        result.Value.Should().BeNull();
    }
}