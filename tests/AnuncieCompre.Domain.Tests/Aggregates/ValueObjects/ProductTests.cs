using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ValueObjects;

public class ProductTests
{
    
    [Theory]
    [InlineData("disco de freio")]
    [InlineData("memoria ram de 8gb")]
    [InlineData("geladeira electrolux frostfree")]
    public void Create_ValidProduct_ShouldCreateProduct(string product)
    {
        Result<Product> result = Product.Create(product);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<Product>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("geladeira electrolux frostfree prata")]
    public void Create_InvalidProduct_ShouldNotCreateProduct(string product)
    {
        Result<Product> result = Product.Create(product);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
    }
}