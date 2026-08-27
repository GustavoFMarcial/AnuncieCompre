using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Tests.Domain.Aggregates.ConversationAggregate.Flows;

public class UserTypeTests
{
    
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("2")]
    public void Create_ValidUserType_ShouldCreateUserType(string userType)
    {
        Result<UserType> result = UserType.Create(userType);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("UserType validado com sucesso");
        result.Value.Should().BeOfType<UserType>();
    }

    [Theory]
    [InlineData("3")]
    [InlineData("99")]
    [InlineData("")]
    public void Create_InvalidUserType_ShouldNotCreateUserType(string userType)
    {
        Result<UserType> result = UserType.Create(userType);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Opção inválida, escolha novamente.");
        result.Value.Should().BeNull();
    }
}