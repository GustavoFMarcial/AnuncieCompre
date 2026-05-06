using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.NodeValidators;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Flows;

public static class CustomerFlow
{
    public static IReadOnlyDictionary<string, IConversationNode> Build()
    {
        IValueObjectValidator cpfValidator = new CpfValidator();
        IValueObjectValidator companyCategoryValidator = new CompanyCategoryValidator();
        IValueObjectValidator quantityValidator = new QuantityValidator();
        IValueObjectValidator productValidator = new ProductValidator();
        // IValueObjectValidator optionValidator = new OptionValidator(["1", "2"]);

        INodeValidator finishValidator = new FinalNodeValidator();
        INodeValidator askAnotherOrderValidator = new OptionNodeValidator(["1", "2"]);
        INodeValidator askQuantityValidator = new ValidationNodeValidator(quantityValidator);
        INodeValidator askProductValidator = new ValidationNodeValidator(productValidator);
        INodeValidator askCompanyCategoryValidator = new ValidationNodeValidator(companyCategoryValidator);
        INodeValidator customerRegisteredValidator = new OptionNodeValidator(["1", "2"]);
        INodeValidator askOrderValidator = new OptionNodeValidator(["1", "2"]);
        INodeValidator askCpf = new ValidationNodeValidator(cpfValidator);

        // IDomainEventFactory customerSentDataToRegisterDomainEventFactory = new CustomerSentDataToRegisterDomainEventFactory();
        // IDomainEventFactory customerSentDataToOrderDomainEventFactory = new CustomerSentDataToOrderDomainEventFactory();
        IDomainEventFactory customerSentCpfDomainEventFactory = new CustomerSentCpfDomainEventFactory();
        IDomainEventFactory customerSentCompanyCategoryDomainEventFactory = new CustomerSentCompanyCategoryDomainEventFactory();
        IDomainEventFactory customerSentProductDomainEventFactory = new CustomerSentProductDomainEventFactory();
        IDomainEventFactory customerSentQuantityDomainEventFactory = new CustomerSentQuantityDomainEventFactory();

        var finish = new FinalNode
        {
            Id = "customer_finish",
            Message = "Ok, até logo!",
            NodeValidator = finishValidator,
        };

        var askAnotherOrder = new OptionNode
        {
            Id = "customer_ask_another_order",
            // Options = ["1", "2"],
            // ValueObjectValidator = optionValidator,
            Message =
                """
                Pedido criado com sucesso!

                Deseja criar outro pedido?
                1 - Sim
                2 - Não
                """,
            NodeValidator = askAnotherOrderValidator,
        };

        var askQuantity = new ValidationNode
        {
            Id = "customer_ask_quantity",
            // Options = null!,
            // ValueObjectValidator = quantityValidator,
            // TempDataType = "Quantity",
            Message = "Qual quantia deseja comprar?",
            NodeValidator = askQuantityValidator,
            DomainEventFactory = customerSentQuantityDomainEventFactory,
        };

        var askProduct = new ValidationNode
        {
            Id = "customer_ask_product",
            // Options = null!,
            // ValueObjectValidator = productValidator,
            // TempDataType = "Product",
            Message = "Qual produto deseja comprar?",
            NodeValidator = askProductValidator,
            DomainEventFactory = customerSentProductDomainEventFactory,
        };

        var askCompanyCategory = new ValidationNode
        {
            Id = "customer_ask_company_category",
            // Options = CompanyCategoryExtensions.ToStringArray(),
            // ValueObjectValidator = companyCategoryValidator,
            // TempDataType = "CompanyCategory",
            Message =
                $"""
                Qual categoria de produto deseja comprar?

                {CompanyCategoryExtensions.PrintNames()}
                """,
            NodeValidator = askCompanyCategoryValidator,
            DomainEventFactory = customerSentCompanyCategoryDomainEventFactory,
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
            // Options = ["1", "2"],
            // ValueObjectValidator = optionValidator,
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!

                Deseja já realizar um pedido?
                1 - Sim
                2 - Não
                """,
            NodeValidator = customerRegisteredValidator,
        };

        var askCPF = new ValidationNode
        {
            Id = "customer_ask_cpf",
            // Options = null!,
            // ValueObjectValidator = cpfValidator,
            // TempDataType = "CPF",
            Message = "Qual seu CPF?",
            NodeValidator = askCpf,
            DomainEventFactory = customerSentCpfDomainEventFactory,
        };

        askCPF.Transitions["next"] = customerRegistered;

        customerRegistered.Transitions["1"] = askCompanyCategory;
        customerRegistered.Transitions["2"] = finish;

        askCompanyCategory.Transitions["next"] = askProduct;
        askProduct.Transitions["next"] = askQuantity;
        askQuantity.Transitions["next"] = askAnotherOrder;

        askAnotherOrder.Transitions["1"] = askCompanyCategory;
        askAnotherOrder.Transitions["2"] = finish;

        askOrder.Transitions["1"] = askCompanyCategory;
        askOrder.Transitions["2"] = finish;

        finish.Transitions["next"] = askOrder;

        return new Dictionary<string, IConversationNode>
        {
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