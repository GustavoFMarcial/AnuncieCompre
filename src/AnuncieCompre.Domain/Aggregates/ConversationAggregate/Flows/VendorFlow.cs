using AnuncieCompre.Domain.Aggregates.UserAggregate;
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
        INodeValidator askConfirmationValidator = new ConfirmationNodeValidator(["1", "2"]);
        INodeValidator askToPremiumValidator = new OptionNodeValidator(["1", "2"]);
        INodeValidator finishValidator = new FinalNodeValidator();

        IDomainEventFactory vendorSentCompanyCategoryDomainEventFactory = new VendorSentCompanyCategoryDomainEventFactory();
        IDomainEventFactory vendorSentCompanyNameDomainEventFactory = new VendorSentCompanyNameDomainEventFactory();
        IDomainEventFactory vendorSentCnpjDomainEventFactory = new VendorSentCnpjDomainEventFactory();
        IDomainEventFactory vendorConfirmedRegistrationDomainEventFactory = new VendorConfirmedRegistrationDomainEventFactory();
        IDomainEventFactory userFinishedConversationDomainEventFactory = new UserFinishedConversationDomainEventFactory();

        var finish = new FinalNode
        {
            Id = "vendor_finish",
            Message = "Ok, até logo!",
            NodeValidator = finishValidator,
            DomainEventFactory = [userFinishedConversationDomainEventFactory],
        };

        var paymentToPremium = new FinalNode
        {
            Id = "vendor_payment_to_premium",
            Message = "FEATURE NÃO IMPLEMENTADA AINDA",
            NodeValidator = finishValidator,
            DomainEventFactory = [userFinishedConversationDomainEventFactory],
        };

        var askPremium = new OptionNode
        {
            Id = "vendor_ask_premium",
            Message =
            """
            Olá, bem-vindo novamente ao AnuncieCompre!

            Deseja assinar nosso plano premium para receber pedidos mais rápido ?            
            
            1 - Sim
            2 - Não
            """,
            NodeValidator = askToPremiumValidator,
        };

        var vendorRegistered = new OptionNode
        {
            Id = "vendor_registered",
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!
                Assim que pedidos compatíveis com sua categoria aparecerem você será notificado.

                Deseja assinar nosso plano premium para receber pedidos mais rápido ?

                1 - Sim
                2 - Não
                """,
            NodeValidator = askToPremiumValidator,
            DomainEventFactory = [],
        };

        var askCNPJ = new ValidationNode
        {
            Id = "vendor_ask_cnpj",
            Message = "Qual o CNPJ da empresa?",
            NodeValidator = askCnpjValidator,
            DomainEventFactory = [vendorSentCnpjDomainEventFactory],
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

        var askRegistrationConfirmation = new ConfirmationNode
        {
            Id = "initial_vendor_ask_confirmation",
            Message =
                """
                Os dados passados estão corretos para que possamos te registrar?

                1 - Sim.
                2 - Não, passar dados novamente.
                """,
            NodeValidator = askConfirmationValidator,
            DomainEventFactory = [vendorConfirmedRegistrationDomainEventFactory],
        };

        conversationflow["initial_ask_user_type"].Transitions["2"] = askCNPJ;

        askCompanyCategory.Transitions["next"] = askCompanyName;

        askCompanyName.Transitions["next"] = askCNPJ;

        askCNPJ.Transitions["next"] = askRegistrationConfirmation;

        askRegistrationConfirmation.Transitions["1"] = vendorRegistered;
        askRegistrationConfirmation.Transitions["2"] = conversationflow["initial_ask_name"];

        vendorRegistered.Transitions["1"] = paymentToPremium;
        vendorRegistered.Transitions["2"] = finish;

        finish.Transitions["next"] = askPremium;
        
        paymentToPremium.Transitions["next"] = askPremium;

        askPremium.Transitions["1"] = paymentToPremium;
        askPremium.Transitions["2"] = finish;

        return new Dictionary<string, IConversationNode>
        {
            { askCompanyCategory.Id, askCompanyCategory },
            { askCompanyName.Id, askCompanyName },
            { askCNPJ.Id, askCNPJ },
            { vendorRegistered.Id, vendorRegistered }
        }.AsReadOnly();
    }
}