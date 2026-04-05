using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.ValueObjectValidator;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

public class ConversationFlow()
{
    public static ConversationNode Build()
    {
        IValidator cnpjValidator = new CnpjValidator();
        IValidator cpfValidator = new CpfValidator();
        IValidator emailValidator = new EmailValidator();
        IValidator nameValidator = new NameValidator();
        IValidator productValidator = new ProductValidator();
        IValidator quantityValidator = new QuantityValidator();
        // ─── Fim ──────────────────────────────────────────────────────────────

        var finish = new ConversationNode
        {
            Message = "Ok, até logo!"
        };

        // ─── Vendor registrado ────────────────────────────────────────────────

        var vendorRegistered = new ConversationNode
        {
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!

                Assim que pedidos compatíveis com sua categoria aparecerem você será notificado.
                """
        };

        // ─── Pedido ───────────────────────────────────────────────────────────

        var askAnotherOrder = new ConversationNode
        {
            Options = ["1", "2"],
            NodeValidator = new OptionValidator(["1", "2"]),
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
            Options = null!,
            NodeValidator = quantityValidator,
            TempDataType = "Quantity",
            Message = "Qual quantia deseja comprar?"
        };

        var askProduct = new ConversationNode()
        {
            Options = null!,
            NodeValidator = productValidator,
            TempDataType = "Product",
            Message = "Qual produto deseja comprar?"
        };

        askProduct.Transitions["next"] = askQuantity;
        askQuantity.Transitions["next"] = askAnotherOrder;

        askAnotherOrder.Transitions["2"] = finish;

        // ─── Categoria ────────────────────────────────────────────────────────

        var askCompanyCategory = new ConversationNode
        {
            Options = CompanyCategoryExtensions.ToStringArray(),
            NodeValidator = new OptionValidator(CompanyCategoryExtensions.ToStringArray()),
            TempDataType = "Category",
            Message =
                $"""
                Qual categoria de produto deseja comprar?

                {CompanyCategoryExtensions.PrintNames()}
                """
        };

        askAnotherOrder.Transitions["1"] = askCompanyCategory;
        askCompanyCategory.Transitions["next"] = askProduct;

        // ─── Registro Customer ────────────────────────────────────────────────

        var customerRegistered = new ConversationNode
        {
            Options = ["1", "2"],
            NodeValidator = new OptionValidator(["1", "2"]),
            Message =
                """
                Obrigado por se registrar no AnuncieCompre!

                Deseja já realizar um pedido?
                1 - Sim
                2 - Não
                """
        };

        customerRegistered.Transitions["1"] = askCompanyCategory;
        customerRegistered.Transitions["2"] = finish;

        var askCPF = new ConversationNode
        {
            Options = null!,
            NodeValidator = cpfValidator,
            TempDataType = "CPF",
            Message = "Qual seu CPF?"
        };

        askCPF.Transitions["next"] = customerRegistered;

        // ─── Registro Vendor ──────────────────────────────────────────────────

        var askCNPJ = new ConversationNode
        {
            Options = null!,
            NodeValidator = cnpjValidator,
            TempDataType = "CNPJ",
            Message = "Qual o CNPJ da empresa?"
        };

        askCNPJ.Transitions["next"] = vendorRegistered;

        // ─── Email ────────────────────────────────────────────────────────────

        var askEmailCustomer = new ConversationNode
        {
            Options = null!,
            NodeValidator = emailValidator,
            TempDataType = "Email",
            Message = "Qual email para cadastro?"
        };

        var askEmailVendor = new ConversationNode
        {
            Options = null!,
            NodeValidator = emailValidator,
            TempDataType = "Email",
            Message = "Qual email para cadastro?"
        };

        askEmailCustomer.Transitions["next"] = askCPF;
        askEmailVendor.Transitions["next"] = askCNPJ;

        // ─── Nome ─────────────────────────────────────────────────────────────

        var askCompanyCategoryVendor = new ConversationNode
        {
            Options = CompanyCategoryExtensions.ToStringArray(),
            NodeValidator = new OptionValidator(CompanyCategoryExtensions.ToStringArray()),
            TempDataType = "Category",
            Message =
                $"""
                Qual o ramo da empresa?

                {CompanyCategoryExtensions.PrintNames()}
                """
        };

        var askCompanyName = new ConversationNode
        {
            Options = null!,
            NodeValidator = nameValidator,
            TempDataType = "Name",
            Message = "Qual o nome da empresa?"
        };

        var askFullName = new ConversationNode
        {
            Options = null!,
            NodeValidator = nameValidator,
            TempDataType = "Name",
            Message = "Qual seu nome completo?"
        };

        askFullName.Transitions["next"] = askEmailCustomer;
        askCompanyName.Transitions["next"] = askEmailVendor;
        askCompanyCategoryVendor.Transitions["next"] = askCompanyName;

        // ─── Tipo de usuário ──────────────────────────────────────────────────

        var askUserType = new ConversationNode
        {
            Options = ["1", "2"],
            NodeValidator = new OptionValidator(["1", "2"]),
            Message =
                """
                Você deseja usar nosso sistema como cliente ou como fornecedor?

                1 - Cliente
                2 - Fornecedor
                """
        };

        askUserType.Transitions["1"] = askFullName;
        askUserType.Transitions["2"] = askCompanyCategoryVendor;

        // ─── Início — novo usuário ────────────────────────────────────────────

        var start = new ConversationNode
        {
            Options = ["1", "2"],
            NodeValidator = new OptionValidator(["1", "2"]),
            Message =
                """
                Olá, bem-vindo! Vimos que é novo por aqui.

                Deseja se registrar no AnuncieCompre para utilizar nosso sistema?
                1 - Sim
                2 - Não
                """
        };

        start.Transitions["1"] = askUserType;
        start.Transitions["2"] = finish;

        return start;
    }
}