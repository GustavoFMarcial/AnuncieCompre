using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Conversation.Flows;

public class ConversationFlow
{
    public static IReadOnlyDictionary<Guid, IConversationNode> Build()
    {
        IValueObjectValidator emailValidator = new EmailValidator();
        IValueObjectValidator nameValidator = new NameValidator();
        IValueObjectValidator quantityValidator = new QuantityValidator();
        IValueObjectValidator productValidator = new ProductValidator();
        IValueObjectValidator companyCategoryValidator = new CompanyCategoryValidator();

        INodeValidator finishValidator = new FinalNodeValidator();
        INodeValidator askAnotherOrderValidator = new OptionNodeValidator(["1", "2"]);
        INodeValidator askEmailValidator = new ValidationNodeValidator(emailValidator);
        INodeValidator askNameValidator = new ValidationNodeValidator(nameValidator);
        INodeValidator askConfirmationValidator = new ConfirmationNodeValidator(["1", "2"]);
        INodeValidator askQuantityValidator = new ValidationNodeValidator(quantityValidator);
        INodeValidator askProductValidator = new ValidationNodeValidator(productValidator);
        INodeValidator askCompanyCategoryValidator = new ValidationNodeValidator(companyCategoryValidator);
        INodeValidator startValidator = new OptionNodeValidator(["1", "2"]);

        IDomainEventFactory userSentNameDomainEventFactory = new CustomerSentNameDomainEventFactory();
        IDomainEventFactory userSentEmailDomainEventFactory = new CustomerSentEmailDomainEventFactory();
        IDomainEventFactory userDoesNotConfirmedRegistrationDomainEventFactory = new CustomerDoesNotConfirmedRegistrationDomainEventFactory();
        IDomainEventFactory userDoesNotConfirmedOrderDomainEventFactory = new CustomerDoesNotConfirmedOrderDomainEventFactory();
        IDomainEventFactory userSentQuantityDomainEventFactory = new CustomerSentQuantityDomainEventFactory();
        IDomainEventFactory userSentProductDomainEventFactory = new CustomerSentProductDomainEventFactory();
        IDomainEventFactory userSentCompanyCategoryDomainEventFactory = new CustomerSentCompanyCategoryDomainEventFactory();
        IDomainEventFactory userFinishedConversationDomainEventFactory = new CustomerFinishedConversationDomainEventFactory();

        var finish = new FinalNode
        {
            Id = Guid.NewGuid(),
            Message = "Ok, até logo!",
        };

        var askAnotherOrder = new ConfirmationNode
        {
            Id = Guid.NewGuid(),
            Message =
                """
                Pedido criado com sucesso!

                Deseja criar outro pedido?
                1 - Sim
                2 - Não
                """,
            NodeValidator = askConfirmationValidator,
            DomainEventFactory = [userFinishedConversationDomainEventFactory],
        };

        var askRegistrationConfirmation = new ConfirmationNode
        {
            Id = Guid.NewGuid(),
            Message =
                """
                Nome e email passados estão corretos para que possamos colocar junto ao pedido ?

                1 - Sim.
                2 - Não, passar dados novamente.
                """,
            NodeValidator = askConfirmationValidator,
            DomainEventFactory = [userDoesNotConfirmedRegistrationDomainEventFactory]
        };

        var askEmail = new ValidationNode
        {
            Id = Guid.NewGuid(),
            Message = "Qual email para cadastro?",
            NodeValidator = askEmailValidator,
            DomainEventFactory = [userSentEmailDomainEventFactory],
        };

        var askName = new ValidationNode
        {
            Id = Guid.NewGuid(),
            Message = "Qual seu nome?",
            NodeValidator = askNameValidator,
            DomainEventFactory = [userSentNameDomainEventFactory],
        };

        var askRegistration = new ConfirmationNode
        {
            Id = Guid.NewGuid(),
            Message =
                """
                Para que possamos prosseguir com o envio do pedido aos fornecedores precisamos de seu nome completo e um email, deseja continuar ?

                1 - Sim.
                2 - Não, finalizar atendimento.
                """,
            NodeValidator = askConfirmationValidator,
            DomainEventFactory = [userFinishedConversationDomainEventFactory],
        };

        var askOrderConfirmation = new ConfirmationNode
        {
            Id = Guid.NewGuid(),
            Message =
                """
                As informações passadas estão corretas para que possamos enviar o pedido aos fornecedores?

                1 - Sim.
                2 - Não, passar informações novamente.
                """,
            NodeValidator = askConfirmationValidator,
            DomainEventFactory = [userDoesNotConfirmedOrderDomainEventFactory]
        };

        var askQuantity = new ValidationNode
        {
            Id = Guid.NewGuid(),
            Message = "Qual quantia deseja comprar?",
            NodeValidator = askQuantityValidator,
            DomainEventFactory = [userSentQuantityDomainEventFactory],
        };

        var askProduct = new ValidationNode
        {
            Id = Guid.NewGuid(),
            Message = "Qual produto deseja comprar?",
            NodeValidator = askProductValidator,
            DomainEventFactory = [userSentProductDomainEventFactory],
        };

        var askCompanyCategory = new ValidationNode
        {
            Id = Guid.NewGuid(),
            Message =
                $"""
                Qual categoria de produto deseja comprar?

                {CompanyCategoryExtensions.PrintNames()}
                """,
            NodeValidator = askCompanyCategoryValidator,
            DomainEventFactory = [userSentCompanyCategoryDomainEventFactory],
        };

        var start = new ConfirmationNode
        {
            Id = Guid.NewGuid(),
            Message =
                """
                Olá, bem-vindo ao AnuncieCompre.

                Deseja criar um pedido de compra ?
                1 - Sim
                2 - Não
                """,
            NodeValidator = askConfirmationValidator,
            DomainEventFactory = [userFinishedConversationDomainEventFactory],
        };

        start.Transitions["1"] = askCompanyCategory;
        start.Transitions["2"] = finish;

        askCompanyCategory.Transitions["next"] = askProduct;

        askProduct.Transitions["next"] = askQuantity;

        askQuantity.Transitions["next"] = askOrderConfirmation;

        askOrderConfirmation.Transitions["1"] = askRegistration;
        askOrderConfirmation.Transitions["2"] = askCompanyCategory;

        askRegistration.Transitions["1"] = askName;
        askRegistration.Transitions["2"] = finish;

        askName.Transitions["next"] = askEmail;
        askEmail.Transitions["next"] = askRegistrationConfirmation;

        askRegistrationConfirmation.Transitions["1"] = askAnotherOrder;
        askRegistrationConfirmation.Transitions["2"] = askName;

        askAnotherOrder.Transitions["1"] = askCompanyCategory;
        askAnotherOrder.Transitions["2"] = finish;

        finish.Transitions["next"] = start;

        return new Dictionary<Guid, IConversationNode>
        {
            { start.Id, start },
            {askCompanyCategory.Id, askCompanyCategory},
            {askProduct.Id, askProduct},
            {askQuantity.Id, askQuantity},
            {askOrderConfirmation.Id, askOrderConfirmation},
            {askRegistration.Id, askRegistration},
            {askName.Id, askName},
            {askEmail.Id, askEmail},
            {askRegistrationConfirmation.Id, askRegistrationConfirmation},
            {askAnotherOrder.Id, askAnotherOrder},
            { finish.Id, finish }
        }.AsReadOnly();
    }
}