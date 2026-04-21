using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

public class ConversationFlow()
{
    public static Dictionary<string, ConversationNode> Build()
    {
        IValueObjectValidator cnpjValidator = new CnpjValidator();
        IValueObjectValidator cpfValidator = new CpfValidator();
        IValueObjectValidator emailValidator = new EmailValidator();
        IValueObjectValidator nameValidator = new NameValidator();
        IValueObjectValidator productValidator = new ProductValidator();
        IValueObjectValidator quantityValidator = new QuantityValidator();
        IValueObjectValidator companyCategoryValidator = new CompanyCategoryValidator();
        IValueObjectValidator userTypeValidator = new UserTypeValidator();
        IValueObjectValidator optionValidator = new OptionValidator(["1", "2"]);

        IDomainEventFactory userSentDataToRegisterDomainEventFactory = new UserSentDataToRegisterDomainEventFactory();
        IDomainEventFactory customerSentDataToRegisterDomainEventFactory = new CustomerSentDataToRegisterDomainEventFactory();
        IDomainEventFactory vendorSentDataToRegisterDomainEventFactory = new VendorSentDataToRegisterDomainEventFactory();
        IDomainEventFactory customerSentDataToOrderDomainEventFactory = new CustomerSentDataToOrderDomainEventFactory();

        // ─── Nodes ────────────────────────────────────────────────────────────

        var finish = new ConversationNode { Id = "1", Message = "Ok, até logo!" };

        var vendorRegistered = new ConversationNode
        {
            Id = "2",
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!

                Assim que pedidos compatíveis com sua categoria aparecerem você será notificado.
                """
        };

        var askAnotherOrder = new ConversationNode
        {
            Id = "3",
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
            Id = "4",
            Options = null!,
            ValueObjectValidator = quantityValidator,
            TempDataType = "Quantity",
            Message = "Qual quantia deseja comprar?",
            DomainEventFactory = customerSentDataToOrderDomainEventFactory,
        };

        var askProduct = new ConversationNode
        {
            Id = "5",
            Options = null!,
            ValueObjectValidator = productValidator,
            TempDataType = "Product",
            Message = "Qual produto deseja comprar?"
        };

        var askCompanyCategory = new ConversationNode
        {
            Id = "6",
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
            Id = "7",
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
            Id = "8",
            Options = null!,
            ValueObjectValidator = cpfValidator,
            TempDataType = "CPF",
            Message = "Qual seu CPF?",
            DomainEventFactory = customerSentDataToRegisterDomainEventFactory,
        };

        var askCNPJ = new ConversationNode
        {
            Id = "9",
            Options = null!,
            ValueObjectValidator = cnpjValidator,
            TempDataType = "CNPJ",
            Message = "Qual o CNPJ da empresa?",
            DomainEventFactory = vendorSentDataToRegisterDomainEventFactory,
        };

        var askEmailCustomer = new ConversationNode
        {
            Id = "10",
            Options = null!,
            ValueObjectValidator = emailValidator,
            TempDataType = "Email",
            Message = "Qual email para cadastro?",
            DomainEventFactory = userSentDataToRegisterDomainEventFactory,
        };

        var askEmailVendor = new ConversationNode
        {
            Id = "11",
            Options = null!,
            ValueObjectValidator = emailValidator,
            TempDataType = "Email",
            Message = "Qual email para cadastro?",
            DomainEventFactory = userSentDataToRegisterDomainEventFactory,
        };

        var askCompanyCategoryVendor = new ConversationNode
        {
            Id = "12",
            Options = CompanyCategoryExtensions.ToStringArray(),
            ValueObjectValidator = companyCategoryValidator,
            TempDataType = "CompanyCategory",
            Message =
                $"""
                Qual o ramo da empresa?

                {CompanyCategoryExtensions.PrintNames()}
                """
        };

        var askCompanyName = new ConversationNode
        {
            Id = "13",
            Options = null!,
            ValueObjectValidator = nameValidator,
            TempDataType = "Name",
            Message = "Qual o nome da empresa?"
        };

        var askFullName = new ConversationNode
        {
            Id = "14",
            Options = null!,
            ValueObjectValidator = nameValidator,
            TempDataType = "Name",
            Message = "Qual seu nome completo?"
        };

        var askUserType = new ConversationNode
        {
            Id = "15",
            Options = ["1", "2"],
            ValueObjectValidator = userTypeValidator,
            TempDataType = "UserType",
            Message =
                """
                Você deseja usar nosso sistema como cliente ou como fornecedor?

                1 - Cliente
                2 - Fornecedor
                """
        };

        var start = new ConversationNode
        {
            Id = "16",
            Options = ["1", "2"],
            ValueObjectValidator = optionValidator,
            Message =
                """
                Olá, bem-vindo! Vimos que é novo por aqui.

                Deseja se registrar no AnuncieCompre para utilizar nosso sistema?
                1 - Sim
                2 - Não
                """
        };

        // ─── Transitions (mantidas) ──────────────────────────────────────────

        askProduct.Transitions["next"] = askQuantity;
        askQuantity.Transitions["next"] = askAnotherOrder;
        askAnotherOrder.Transitions["2"] = finish;

        askAnotherOrder.Transitions["1"] = askCompanyCategory;
        askCompanyCategory.Transitions["next"] = askProduct;

        customerRegistered.Transitions["1"] = askCompanyCategory;
        customerRegistered.Transitions["2"] = finish;

        askCPF.Transitions["next"] = customerRegistered;
        askCNPJ.Transitions["next"] = vendorRegistered;

        askEmailCustomer.Transitions["next"] = askCPF;
        askEmailVendor.Transitions["next"] = askCNPJ;

        askFullName.Transitions["next"] = askEmailCustomer;
        askCompanyName.Transitions["next"] = askEmailVendor;
        askCompanyCategoryVendor.Transitions["next"] = askCompanyName;

        askUserType.Transitions["1"] = askFullName;
        askUserType.Transitions["2"] = askCompanyCategoryVendor;

        start.Transitions["1"] = askUserType;
        start.Transitions["2"] = finish;

        // ─── Dictionary ──────────────────────────────────────────────────────

        return new Dictionary<string, ConversationNode>
        {
            { finish.Id, finish },
            { vendorRegistered.Id, vendorRegistered },
            { askAnotherOrder.Id, askAnotherOrder },
            { askQuantity.Id, askQuantity },
            { askProduct.Id, askProduct },
            { askCompanyCategory.Id, askCompanyCategory },
            { customerRegistered.Id, customerRegistered },
            { askCPF.Id, askCPF },
            { askCNPJ.Id, askCNPJ },
            { askEmailCustomer.Id, askEmailCustomer },
            { askEmailVendor.Id, askEmailVendor },
            { askCompanyCategoryVendor.Id, askCompanyCategoryVendor },
            { askCompanyName.Id, askCompanyName },
            { askFullName.Id, askFullName },
            { askUserType.Id, askUserType },
            { start.Id, start }
        };
    }
}