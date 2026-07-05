using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Conversation.Flows;

public class VendorFlow
{
    public static IReadOnlyDictionary<string, IConversationNode> Build(IReadOnlyDictionary<string, IConversationNode> conversationflow)
    {
        IValueObjectValidator cnpjValidator = new CnpjValidator();
        IValueObjectValidator nameValidator = new NameValidator();
        IValueObjectValidator companyCategoryValidator = new CompanyCategoryValidator();

        INodeValidator askCnpjValidator = new ValidationNodeValidator(cnpjValidator);
        INodeValidator askCompanyNameValidator = new ValidationNodeValidator(nameValidator);
        INodeValidator askCompanyCategoryValidator = new ValidationNodeValidator(companyCategoryValidator);
        // INodeValidator askConfirmationValidator = new OptionNodeValidator(["1", "2"]);

        IDomainEventFactory vendorSentCompanyCategoryDomainEventFactory = new VendorSentCompanyCategoryDomainEventFactory();
        IDomainEventFactory vendorSentCompanyNameDomainEventFactory = new VendorSentCompanyNameDomainEventFactory();
        IDomainEventFactory vendorSentCnpjDomainEventFactory = new VendorSentCnpjDomainEventFactory();
        IDomainEventFactory vendorConfirmedRegistrationDomainEventFactory = new VendorConfirmedRegistrationDomainEventFactory();
        IDomainEventFactory userFinishedConversationDomainEventFactory = new UserFinishedConversationDomainEventFactory();

        var vendorRegistered = new FinalNode
        {
            Id = "vendor_registered",
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!

                Assim que pedidos compatíveis com sua categoria aparecerem você será notificado.
                """,
            DomainEventFactory = [userFinishedConversationDomainEventFactory],
        };

        var askCNPJ = new ValidationNode
        {
            Id = "vendor_ask_cnpj",
            Message = "Qual o CNPJ da empresa?",
            NodeValidator = askCnpjValidator,
            DomainEventFactory = [vendorSentCnpjDomainEventFactory, vendorConfirmedRegistrationDomainEventFactory],
        };

        var askCompanyName = new ValidationNode
        {
            Id = "vendor_ask_company_name",
            NodeValidator = askCompanyNameValidator,
            Message = "Qual o nome da empresa?",
            DomainEventFactory = [vendorSentCompanyNameDomainEventFactory],
        };

        var askCompanyCategory = new ValidationNode
        {
            Id = "vendor_ask_company_category",
            Message =
                $"""
                Qual o ramo da empresa?

                {CompanyCategoryExtensions.PrintNames()}
                """,
            NodeValidator = askCompanyCategoryValidator,
            DomainEventFactory = [vendorSentCompanyCategoryDomainEventFactory],
        };

        // var askConfirmation = new OptionNode
        // {
        //     Id = "initial_vendor_ask_confirmation",
        //     Message =
        //         """
        //         Os dados passados estão corretos para que possamos te registrar?

        //         1 - Sim.
        //         2 - Não, passar dados novamente.
        //         """,
        //     NodeValidator = askConfirmationValidator,
        // };

        conversationflow["initial_ask_user_type"].Transitions["2"] = askCNPJ;

        // askConfirmation.Transitions["1"] = askCNPJ;
        // askConfirmation.Transitions["2"] = conversationflow["initial_ask_name"];

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