using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using FluentAssertions;

namespace AnuncieCompre.Domain.Tests.Aggregates.ValueObjects;

public class CompanyCategoryTests
{

    [Theory]
    [InlineData("1")]
    [InlineData("3")]
    [InlineData("5")]
    public void Create_ValidCompanyCategory_ShouldCreateCompanyCategory(string companyCategory)
    {
        Result<CompanyCategory> result = CompanyCategory.Create(companyCategory);

        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("CompanyCategory validado com sucesso");
        result.Value.Should().BeOfType<CompanyCategory>();
    }

    [Theory]
    [InlineData("99")]
    [InlineData("")]
    [InlineData("@")]
    public void Create_InvalidCompanyCategory_ShouldNotCreateCompanyCategory(string companyCategory)
    {
        Result<CompanyCategory> result = CompanyCategory.Create(companyCategory);

        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Opção inválida, escolha novamente.");
        result.Value.Should().BeNull();
    }
}