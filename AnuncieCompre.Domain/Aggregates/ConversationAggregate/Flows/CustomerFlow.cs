using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Flows;

public static class CustomerFlow
{
    public static Dictionary<string, ConversationNode> Build()
    {
        IValueObjectValidator cpfValidator = new CpfValidator();
        IValueObjectValidator productValidator = new ProductValidator();
        IValueObjectValidator quantityValidator = new QuantityValidator();
        IValueObjectValidator companyCategoryValidator = new CompanyCategoryValidator();
        IValueObjectValidator optionValidator = new OptionValidator(["1", "2"]);

        IDomainEventFactory customerSentDataToRegisterDomainEventFactory = new CustomerSentDataToRegisterDomainEventFactory();
        IDomainEventFactory customerSentDataToOrderDomainEventFactory = new CustomerSentDataToOrderDomainEventFactory();

        var finish = new ConversationNode
        {
            Id = "customer_finish",
            Message = "Ok, até logo!"
        };

        var askAnotherOrder = new ConversationNode
        {
            Id = "customer_ask_another_order",
            Options = ["1", "2"],
            ValueObjectValidator = optionValidator,
            Message =
                """
                Pedido criado com sucesso!

                Deseja criar outro pedido?
                1 - Sim
                2 - Não
                """
        };

        var askQuantity = new ConversationNode
        {
            Id = "customer_ask_quantity",
            Options = null!,
            ValueObjectValidator = quantityValidator,
            TempDataType = "Quantity",
            Message = "Qual quantia deseja comprar?",
            DomainEventFactory = customerSentDataToOrderDomainEventFactory,
        };

        var askProduct = new ConversationNode
        {
            Id = "customer_ask_product",
            Options = null!,
            ValueObjectValidator = productValidator,
            TempDataType = "Product",
            Message = "Qual produto deseja comprar?"
        };

        var askCompanyCategory = new ConversationNode
        {
            Id = "customer_ask_company_category",
            Options = CompanyCategoryExtensions.ToStringArray(),
            ValueObjectValidator = companyCategoryValidator,
            TempDataType = "CompanyCategory",
            Message =
                $"""
                Qual categoria de produto deseja comprar?

                {CompanyCategoryExtensions.PrintNames()}
                """
        };

        var customerRegistered = new ConversationNode
        {
            Id = "customer_registered",
            Options = ["1", "2"],
            ValueObjectValidator = optionValidator,
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!

                Deseja já realizar um pedido?
                1 - Sim
                2 - Não
                """
        };

        var askCPF = new ConversationNode
        {
            Id = "customer_ask_cpf",
            Options = null!,
            ValueObjectValidator = cpfValidator,
            TempDataType = "CPF",
            Message = "Qual seu CPF?",
            DomainEventFactory = customerSentDataToRegisterDomainEventFactory,
        };

        askCPF.Transitions["next"] = customerRegistered;

        customerRegistered.Transitions["1"] = askCompanyCategory;
        customerRegistered.Transitions["2"] = finish;

        askCompanyCategory.Transitions["next"] = askProduct;
        askProduct.Transitions["next"] = askQuantity;
        askQuantity.Transitions["next"] = askAnotherOrder;

        askAnotherOrder.Transitions["1"] = askCompanyCategory;
        askAnotherOrder.Transitions["2"] = finish;

        return new Dictionary<string, ConversationNode>
        {
            { askCPF.Id, askCPF },
            { customerRegistered.Id, customerRegistered },
            { askCompanyCategory.Id, askCompanyCategory },
            { askProduct.Id, askProduct },
            { askQuantity.Id, askQuantity },
            { askAnotherOrder.Id, askAnotherOrder },
            { finish.Id, finish }
        };
    }
}