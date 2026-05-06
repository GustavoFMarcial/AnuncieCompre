using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.NodeValidators;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Flows;

public static class VendorFlow
{
    public static IReadOnlyDictionary<string, IConversationNode> Build()
    {
        IValueObjectValidator cnpjValidator = new CnpjValidator();
        IValueObjectValidator nameValidator = new NameValidator();
        IValueObjectValidator companyCategoryValidator = new CompanyCategoryValidator();

        INodeValidator askCnpjValidator = new ValidationNodeValidator(cnpjValidator);
        INodeValidator askCompanyNameValidator = new ValidationNodeValidator(nameValidator);
        INodeValidator askCompanyCategoryValidator = new ValidationNodeValidator(companyCategoryValidator);

        // IDomainEventFactory vendorSentDataToRegisterDomainEventFactory = new VendorSentDataToRegisterDomainEventFactory();
        IDomainEventFactory vendorSentCompanyCategoryDomainEventFactory = new VendorSentCompanyCategoryDomainEventFactory();
        IDomainEventFactory vendorSentNameDomainEventFactory = new VendorSentNameDomainEventFactory();
        IDomainEventFactory vendorSentCnpjDomainEventFactory = new VendorSentCnpjDomainEventFactory();

        var vendorRegistered = new FinalNode
        {
            Id = "vendor_registered",
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!

                Assim que pedidos compatíveis com sua categoria aparecerem você será notificado.
                """
        };

        var askCNPJ = new ValidationNode
        {
            Id = "vendor_ask_cnpj",
            // Options = null!,
            // ValueObjectValidator = cnpjValidator,
            // TempDataType = "CNPJ",
            Message = "Qual o CNPJ da empresa?",
            NodeValidator = askCnpjValidator,
            DomainEventFactory = vendorSentCnpjDomainEventFactory,
        };

        var askCompanyName = new ValidationNode
        {
            Id = "vendor_ask_company_name",
            // Options = null!,
            // ValueObjectValidator = nameValidator,
            // TempDataType = "CompanyName",
            NodeValidator = askCompanyNameValidator,
            Message = "Qual o nome da empresa?",
            DomainEventFactory = vendorSentNameDomainEventFactory,
        };

        var askCompanyCategory = new ValidationNode
        {
            Id = "vendor_ask_company_category",
            // Options = CompanyCategoryExtensions.ToStringArray(),
            // ValueObjectValidator = companyCategoryValidator,
            // TempDataType = "CompanyCategory",
            Message =
                $"""
                Qual o ramo da empresa?

                {CompanyCategoryExtensions.PrintNames()}
                """,
            NodeValidator = askCompanyCategoryValidator,
            DomainEventFactory = vendorSentCompanyCategoryDomainEventFactory,
        };

        askCompanyCategory.Transitions["next"] = askCompanyName;
        askCompanyName.Transitions["next"] = askCNPJ;
        askCNPJ.Transitions["next"] = vendorRegistered;

        return new Dictionary<string, IConversationNode>
        {
            { askCompanyCategory.Id, askCompanyCategory },
            { askCompanyName.Id, askCompanyName },
            { askCNPJ.Id, askCNPJ },
            { vendorRegistered.Id, vendorRegistered }
        }.AsReadOnly();
    }
}