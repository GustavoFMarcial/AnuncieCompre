using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public static class ConversationFlow
{
    public static ConversationNode Build()
    {
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
            HasValidation = true,
            ValidationType = typeof(VOQuantity),
            HasTempData = true,
            TempDataType = "Quantity",
            Message = "Qual quantia deseja comprar?"
        };

        var askProduct = new ConversationNode
        {
            HasValidation = true,
            ValidationType = typeof(VOProduct),
            HasTempData = true,
            TempDataType = "Product",
            Message = "Qual produto deseja comprar?"
        };

        askProduct.Transitions["next"] = askQuantity;
        askQuantity.Transitions["next"] = askAnotherOrder;

        askAnotherOrder.Transitions["2"] = finish;

        // ─── Categoria ────────────────────────────────────────────────────────

        var askCompanyCategory = new ConversationNode
        {
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
            HasValidation = true,
            ValidationType = typeof(VOCPF),
            HasTempData = true,
            TempDataType = "CPF",
            Message = "Qual seu CPF?"
        };

        askCPF.Transitions["next"] = customerRegistered;

        // ─── Registro Vendor ──────────────────────────────────────────────────

        var askCNPJ = new ConversationNode
        {
            HasValidation = true,
            ValidationType = typeof(VOCNPJ),
            HasTempData = true,
            TempDataType = "CNPJ",
            Message = "Qual o CNPJ da empresa?"
        };

        askCNPJ.Transitions["next"] = vendorRegistered;

        // ─── Email ────────────────────────────────────────────────────────────

        var askEmailCustomer = new ConversationNode
        {
            HasValidation = true,
            ValidationType = typeof(VOEmail),
            HasTempData = true,
            TempDataType = "Email",
            Message = "Qual email para cadastro?"
        };

        var askEmailVendor = new ConversationNode
        {
            HasValidation = true,
            ValidationType = typeof(VOEmail),
            HasTempData = true,
            TempDataType = "Email",
            Message = "Qual email para cadastro?"
        };

        askEmailCustomer.Transitions["next"] = askCPF;
        askEmailVendor.Transitions["next"] = askCNPJ;

        // ─── Nome ─────────────────────────────────────────────────────────────

        var askCompanyCategoryVendor = new ConversationNode
        {
            Message =
                $"""
                Qual o ramo da empresa?

                {CompanyCategoryExtensions.PrintNames()}
                """
        };

        var askCompanyName = new ConversationNode
        {
            HasValidation = true,
            ValidationType = typeof(VOName),
            HasTempData = true,
            TempDataType = "Name",
            Message = "Qual o nome da empresa?"
        };

        var askFullName = new ConversationNode
        {
            HasValidation = true,
            ValidationType = typeof(VOName),
            HasTempData = true,
            TempDataType = "Name",
            Message = "Qual seu nome completo?"
        };

        askFullName.Transitions["next"] = askEmailCustomer;
        askCompanyName.Transitions["next"] = askEmailVendor;
        askCompanyCategoryVendor.Transitions["next"] = askCompanyName;

        // ─── Tipo de usuário ──────────────────────────────────────────────────

        var askUserType = new ConversationNode
        {
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