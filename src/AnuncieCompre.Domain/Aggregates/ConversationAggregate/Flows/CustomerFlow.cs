using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Conversation.Flows;

public class CustomerFlow
{
    public static IReadOnlyDictionary<string, IConversationNode> Build(IReadOnlyDictionary<string, IConversationNode> conversationflow)
    {
        IValueObjectValidator cpfValidator = new CpfValidator();
        IValueObjectValidator companyCategoryValidator = new CompanyCategoryValidator();
        IValueObjectValidator quantityValidator = new QuantityValidator();
        IValueObjectValidator productValidator = new ProductValidator();

        INodeValidator finishValidator = new FinalNodeValidator();
        INodeValidator askAnotherOrderValidator = new OptionNodeValidator(["1", "2"]);
        INodeValidator askQuantityValidator = new ValidationNodeValidator(quantityValidator);
        INodeValidator askProductValidator = new ValidationNodeValidator(productValidator);
        INodeValidator askCompanyCategoryValidator = new ValidationNodeValidator(companyCategoryValidator);
        INodeValidator customerRegisteredValidator = new OptionNodeValidator(["1", "2"]);
        INodeValidator askOrderValidator = new OptionNodeValidator(["1", "2"]);
        INodeValidator askCpf = new ValidationNodeValidator(cpfValidator);
        INodeValidator askConfirmationValidator = new ConfirmationNodeValidator(["1", "2"]); 
;
        IDomainEventFactory userFinishedConversationDomainEventFactory = new UserFinishedConversationDomainEventFactory();
        IDomainEventFactory customerSentCpfDomainEventFactory = new CustomerSentCpfDomainEventFactory();
        IDomainEventFactory customerConfirmedRegistrationDomainEventFactory = new CustomerConfirmedRegistrationDomainEventFactory();
        IDomainEventFactory customerSentCompanyCategoryDomainEventFactory = new CustomerSentCompanyCategoryDomainEventFactory();
        IDomainEventFactory customerSentProductDomainEventFactory = new CustomerSentProductDomainEventFactory();
        IDomainEventFactory customerSentQuantityDomainEventFactory = new CustomerSentQuantityDomainEventFactory();
        IDomainEventFactory customerConfirmedOrderDomainEventFactory = new CustomerConfirmedOrderDomainEventFactory();

        var finish = new FinalNode
        {
            Id = "customer_finish",
            Message = "Ok, até logo!",
            NodeValidator = finishValidator,
            DomainEventFactory = [userFinishedConversationDomainEventFactory],
        };

        var askAnotherOrder = new OptionNode
        {
            Id = "customer_ask_another_order",
            Message =
                """
                Pedido criado com sucesso!

                Deseja criar outro pedido?
                1 - Sim
                2 - Não
                """,
            NodeValidator = askAnotherOrderValidator,
        };

        var askOrderConfirmation = new ConfirmationNode
        {
            Id = "customer_order_ask_confirmation",
            Message =
                """
                As informações passadas estão corretas para que possamos disparar o pedido aos fornecedores?

                1 - Sim.
                2 - Não, passar informações novamente.
                """,
            NodeValidator = askConfirmationValidator,
            DomainEventFactory = [customerConfirmedOrderDomainEventFactory]
        };

        var askQuantity = new ValidationNode
        {
            Id = "customer_ask_quantity",
            Message = "Qual quantia deseja comprar?",
            NodeValidator = askQuantityValidator,
            DomainEventFactory = [customerSentQuantityDomainEventFactory],
        };

        var askProduct = new ValidationNode
        {
            Id = "customer_ask_product",
            Message = "Qual produto deseja comprar?",
            NodeValidator = askProductValidator,
            DomainEventFactory = [customerSentProductDomainEventFactory],
        };

        var askCompanyCategory = new ValidationNode
        {
            Id = "customer_ask_company_category",
            Message =
                $"""
                Qual categoria de produto deseja comprar?

                {CompanyCategoryExtensions.PrintNames()}
                """,
            NodeValidator = askCompanyCategoryValidator,
            DomainEventFactory = [customerSentCompanyCategoryDomainEventFactory],
        };

        var askOrder = new OptionNode
        {
            Id = "ask_order",
            Message =
                """
                Olá, bem-vindo novamente ao AnuncieCompre!

                Deseja realizer um pedido?
                1 - Sim
                2 - Não      
                """,
            NodeValidator = askOrderValidator,
        };

        var customerRegistered = new OptionNode
        {
            Id = "customer_registered",
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!

                Deseja já realizar um pedido?
                1 - Sim
                2 - Não
                """,
            NodeValidator = customerRegisteredValidator,
        };

        var askRegistrationConfirmation = new ConfirmationNode
        {
            Id = "customer_registration_ask_confirmation",
            Message =
                """
                Os dados passados estão corretos para que possamos te registrar?

                1 - Sim.
                2 - Não, passar dados novamente.
                """,
            NodeValidator = askConfirmationValidator,
            DomainEventFactory = [customerConfirmedRegistrationDomainEventFactory]
        };

        var askCPF = new ValidationNode
        {
            Id = "customer_ask_cpf",
            Message = "Qual seu CPF?",
            NodeValidator = askCpf,
            DomainEventFactory = [customerSentCpfDomainEventFactory],
        };

        conversationflow["initial_ask_user_type"].Transitions["1"] = askCPF;

        askCPF.Transitions["next"] = askRegistrationConfirmation;

        askRegistrationConfirmation.Transitions["1"] = customerRegistered;
        askRegistrationConfirmation.Transitions["2"] = conversationflow["initial_ask_name"];

        customerRegistered.Transitions["1"] = askCompanyCategory;
        customerRegistered.Transitions["2"] = finish;

        askCompanyCategory.Transitions["next"] = askProduct;

        askProduct.Transitions["next"] = askQuantity;

        askQuantity.Transitions["next"] = askOrderConfirmation;

        askOrderConfirmation.Transitions["1"] = askAnotherOrder;
        askOrderConfirmation.Transitions["2"] = askCompanyCategory;

        askAnotherOrder.Transitions["1"] = askCompanyCategory;
        askAnotherOrder.Transitions["2"] = finish;

        askOrder.Transitions["1"] = askCompanyCategory;
        askOrder.Transitions["2"] = finish;

        finish.Transitions["next"] = askOrder;

        return new Dictionary<string, IConversationNode>
        {
            { askOrderConfirmation.Id, askOrderConfirmation },
            { askRegistrationConfirmation.Id, askRegistrationConfirmation },
            { askOrder.Id, askOrder },
            { askCPF.Id, askCPF },
            { customerRegistered.Id, customerRegistered },
            { askCompanyCategory.Id, askCompanyCategory },
            { askProduct.Id, askProduct },
            { askQuantity.Id, askQuantity },
            { askAnotherOrder.Id, askAnotherOrder },
            { finish.Id, finish }
        }.AsReadOnly();
    }
}