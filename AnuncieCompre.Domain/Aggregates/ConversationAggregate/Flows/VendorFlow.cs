using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Flows;

public static class VendorFlow
{
    public static Dictionary<string, ConversationNode> Build()
    {
        IValueObjectValidator cnpjValidator = new CnpjValidator();
        IValueObjectValidator nameValidator = new NameValidator();
        IValueObjectValidator companyCategoryValidator = new CompanyCategoryValidator();

        IDomainEventFactory vendorSentDataToRegisterDomainEventFactory = new VendorSentDataToRegisterDomainEventFactory();

        var vendorRegistered = new ConversationNode
        {
            Id = "vendor_registered",
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!

                Assim que pedidos compatíveis com sua categoria aparecerem você será notificado.
                """
        };

        var askCNPJ = new ConversationNode
        {
            Id = "vendor_ask_cnpj",
            Options = null!,
            ValueObjectValidator = cnpjValidator,
            TempDataType = "CNPJ",
            Message = "Qual o CNPJ da empresa?",
            DomainEventFactory = vendorSentDataToRegisterDomainEventFactory,
        };

        var askCompanyName = new ConversationNode
        {
            Id = "vendor_ask_company_name",
            Options = null!,
            ValueObjectValidator = nameValidator,
            TempDataType = "CompanyName",
            Message = "Qual o nome da empresa?"
        };

        var askCompanyCategory = new ConversationNode
        {
            Id = "vendor_ask_company_category",
            Options = CompanyCategoryExtensions.ToStringArray(),
            ValueObjectValidator = companyCategoryValidator,
            TempDataType = "CompanyCategory",
            Message =
                $"""
                Qual o ramo da empresa?

                {CompanyCategoryExtensions.PrintNames()}
                """
        };

        askCompanyCategory.Transitions["next"] = askCompanyName;
        askCompanyName.Transitions["next"] = askCNPJ;
        askCNPJ.Transitions["next"] = vendorRegistered;

        return new Dictionary<string, ConversationNode>
        {
            { askCompanyCategory.Id, askCompanyCategory },
            { askCompanyName.Id, askCompanyName },
            { askCNPJ.Id, askCNPJ },
            { vendorRegistered.Id, vendorRegistered }
        };
    }
}