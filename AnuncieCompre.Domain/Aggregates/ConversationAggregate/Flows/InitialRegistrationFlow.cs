using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Flows;

public static class InitialRegistrationFlow
{
    public static Dictionary<string, ConversationNode> Build()
    {
        IValueObjectValidator nameValidator = new NameValidator();
        IValueObjectValidator emailValidator = new EmailValidator();
        IValueObjectValidator userTypeValidator = new UserTypeValidator();
        IValueObjectValidator optionValidator = new OptionValidator(["1", "2"]);

        IDomainEventFactory userSentDataToRegisterDomainEventFactory = new UserSentDataToRegisterDomainEventFactory();

        var finish = new ConversationNode
        {
            Id = "initial_finish",
            Message = "Ok, até logo!"
        };

        var askUserType = new ConversationNode
        {
            Id = "initial_ask_user_type",
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

        var askEmail = new ConversationNode
        {
            Id = "initial_ask_email",
            Options = null!,
            ValueObjectValidator = emailValidator,
            TempDataType = "Email",
            Message = "Qual email para cadastro?",
            DomainEventFactory = userSentDataToRegisterDomainEventFactory,
        };

        var askName = new ConversationNode
        {
            Id = "initial_ask_name",
            Options = null!,
            ValueObjectValidator = nameValidator,
            TempDataType = "Name",
            Message = "Qual seu nome ou nome da empresa?"
        };

        var start = new ConversationNode
        {
            Id = "initial_start",
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

        start.Transitions["1"] = askName;
        start.Transitions["2"] = finish;

        askName.Transitions["next"] = askEmail;
        askEmail.Transitions["next"] = askUserType;

        return new Dictionary<string, ConversationNode>
        {
            { start.Id, start },
            { askName.Id, askName },
            { askEmail.Id, askEmail },
            { askUserType.Id, askUserType },
            { finish.Id, finish }
        };
    }
}