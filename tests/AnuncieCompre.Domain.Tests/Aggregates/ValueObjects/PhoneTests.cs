using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ValueObjects;

public class PhoneTests
{
    
    [Theory]
    [InlineData("whatsapp:+556592660005")]
    [InlineData("whatsapp:+556593272726")]
    [InlineData("whatsapp:+556593538732")]
    public void Create_ValidPhone_ShouldCreatePhone(string phone)
    {
        Result<Phone> result = Phone.Create(phone);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("VOPhone criado com sucesso");
        result.Value.Should().BeOfType<Phone>();
    }

    [Theory]
    [InlineData("telegram:+556592660005")]
    [InlineData("whatsapp:+796593272726")]
    [InlineData("whatsapp:6593538732")]
    public void Create_InvalidPhone_ShouldNotCreatePhone(string phone)
    {
        Result<Phone> result = Phone.Create(phone);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}