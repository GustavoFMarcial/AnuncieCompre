using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public static class ConversationFlow
{
    public static ConversationNode Build()
    {
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

        var askUserType = new ConversationNode
        {
            Message =
                """
                Você deseja usar nosso sistema como cliente ou como fornecedor?

                1 - Cliente
                2 - Fornecedor
                """
        };

        var finish = new ConversationNode
        {
            Message = "Ok, até logo!",
        };

        start.Transitions["1"] = askUserType;
        start.Transitions["2"] = finish;

        var askFullName = new ConversationNode
        {
            Message = "Qual seu nome completo?"
        };

        var askCompanyCategory = new ConversationNode
        {
            HasValidation = true,
            ValidationType = typeof(VOCNPJ),
            Message =
                $"""
                Qual o ramo da empresa?

                {CompanyCategoryExtensions.PrintNames()}
                """
        };

        askUserType.Transitions["1"] = askFullName;
        askUserType.Transitions["2"] = askCompanyCategory;

        var askEmail = new ConversationNode
        {
            Message = "Qual email para cadastro?"
        };

        return start;
    }
}