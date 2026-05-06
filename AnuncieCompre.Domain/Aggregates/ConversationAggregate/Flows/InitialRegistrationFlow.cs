using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.NodeValidators;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.Flows;

public static class InitialRegistrationFlow
{
    public static IReadOnlyDictionary<string, IConversationNode> Build()
    {
        IValueObjectValidator userTypeValidator = new UserTypeValidator();
        IValueObjectValidator emailValidator = new EmailValidator();
        IValueObjectValidator nameValidator = new NameValidator();
        // IValueObjectValidator optionValidator = new OptionValidator(["1", "2"]);

        INodeValidator askUserTypeValidator = new ValidationNodeValidator(userTypeValidator);
        INodeValidator askEmailValidator = new ValidationNodeValidator(emailValidator);
        INodeValidator askNameValidator = new ValidationNodeValidator(nameValidator);
        INodeValidator startValidator = new OptionNodeValidator(["1", "2"]);

        // IDomainEventFactory userSentDataToRegisterDomainEventFactory = new UserSentDataToRegisterDomainEventFactory();
        IDomainEventFactory userSentNameDomainEventFactory = new UserSentNameDomainEventFactory();
        IDomainEventFactory userSentEmailDomainEventFactory = new UserSentEmailDomainEventFactory();
        IDomainEventFactory userSentTypeDomainEventFactory = new UserSentTypeDomainEventFactory();

        var finish = new FinalNode
        {
            Id = "initial_finish",
            Message = "Ok, até logo!"
        };

        var askUserType = new ValidationNode
        {
            Id = "initial_ask_user_type",
            // Options = ["1", "2"],
            // ValueObjectValidator = userTypeValidator,
            NodeValidator = askUserTypeValidator,
            // TempDataType = "UserType",
            Message =
                """
                Você deseja usar nosso sistema como cliente ou como fornecedor?

                1 - Cliente
                2 - Fornecedor
                """,
            DomainEventFactory = userSentTypeDomainEventFactory,
        };

        var askEmail = new ValidationNode
        {
            Id = "initial_ask_email",
            // Options = null!,
            // ValueObjectValidator = emailValidator,
            // TempDataType = "Email",
            Message = "Qual email para cadastro?",
            NodeValidator = askEmailValidator,
            DomainEventFactory = userSentEmailDomainEventFactory,
        };

        var askName = new ValidationNode
        {
            Id = "initial_ask_name",
            // Options = null!,
            // ValueObjectValidator = nameValidator,
            // TempDataType = "Name",
            Message = "Qual seu nome?",
            NodeValidator = askNameValidator,
            DomainEventFactory = userSentNameDomainEventFactory,
        };

        var start = new OptionNode
        {
            Id = "initial_start",
            // Options = ["1", "2"],
            // ValueObjectValidator = optionValidator,
            Message =
                """
                Olá, bem-vindo! Vimos que é novo por aqui.

                Deseja se registrar no AnuncieCompre para utilizar nosso sistema?
                1 - Sim
                2 - Não
                """,
            NodeValidator = startValidator,
        };

        start.Transitions["1"] = askName;
        start.Transitions["2"] = finish;

        askName.Transitions["next"] = askEmail;
        askEmail.Transitions["next"] = askUserType;

        finish.Transitions["next"] = start;

        return new Dictionary<string, IConversationNode>
        {
            { start.Id, start },
            { askName.Id, askName },
            { askEmail.Id, askEmail },
            { askUserType.Id, askUserType },
            { finish.Id, finish }
        }.AsReadOnly();
    }
}