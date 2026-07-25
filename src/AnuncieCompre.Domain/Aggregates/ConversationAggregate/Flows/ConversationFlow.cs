using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.DomainEventFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Conversation.Flows;

public class ConversationFlow
{
    public static IReadOnlyDictionary<string, IConversationNode> Build()
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

        IDomainEventFactory userSentNameDomainEventFactory = new UserSentNameDomainEventFactory();
        IDomainEventFactory userSentEmailDomainEventFactory = new UserSentEmailDomainEventFactory();
        IDomainEventFactory userDoesNotConfirmedRegistrationDomainEventFactory = new UserDoesNotConfirmedRegistrationDomainEventFactory();
        IDomainEventFactory userDoesNotConfirmedOrderDomainEventFactory = new UserDoesNotConfirmedOrderDomainEventFactory();
        IDomainEventFactory userSentQuantityDomainEventFactory = new UserSentQuantityDomainEventFactory();
        IDomainEventFactory userSentProductDomainEventFactory = new UserSentProductDomainEventFactory();
        IDomainEventFactory userSentCompanyCategoryDomainEventFactory = new UserSentCompanyCategoryDomainEventFactory();
        IDomainEventFactory userFinishedConversationDomainEventFactory = new UserFinishedConversationDomainEventFactory();

        var finish = new FinalNode
        {
            Id = "finish",
            Message = "Ok, até logo!",
            NodeValidator = finishValidator,
            DomainEventFactory = [userFinishedConversationDomainEventFactory],
        };

        var askAnotherOrder = new OptionNode
        {
            Id = "ask_another_order",
            Message =
                """
                Pedido criado com sucesso!

                Deseja criar outro pedido?
                1 - Sim
                2 - Não
                """,
            NodeValidator = askAnotherOrderValidator,
        };

        var askRegistrationConfirmation = new ConfirmationNode
        {
            Id = "registration_ask_confirmation",
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
            Id = "ask_email",
            Message = "Qual email para cadastro?",
            NodeValidator = askEmailValidator,
            DomainEventFactory = [userSentEmailDomainEventFactory],
        };

        var askName = new ValidationNode
        {
            Id = "ask_name",
            Message = "Qual seu nome?",
            NodeValidator = askNameValidator,
            DomainEventFactory = [userSentNameDomainEventFactory],
        };

        var askRegistration = new ConfirmationNode
        {
            Id = "ask_registration",
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
            Id = "order_ask_confirmation",
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
            Id = "ask_quantity",
            Message = "Qual quantia deseja comprar?",
            NodeValidator = askQuantityValidator,
            DomainEventFactory = [userSentQuantityDomainEventFactory],
        };

        var askProduct = new ValidationNode
        {
            Id = "ask_product",
            Message = "Qual produto deseja comprar?",
            NodeValidator = askProductValidator,
            DomainEventFactory = [userSentProductDomainEventFactory],
        };

        var askCompanyCategory = new ValidationNode
        {
            Id = "ask_company_category",
            Message =
                $"""
                Qual categoria de produto deseja comprar?

                {CompanyCategoryExtensions.PrintNames()}
                """,
            NodeValidator = askCompanyCategoryValidator,
            DomainEventFactory = [userSentCompanyCategoryDomainEventFactory],
        };

        var start = new OptionNode
        {
            Id = "start",
            Message =
                """
                Olá, bem-vindo ao AnuncieCompre.

                Deseja criar um pedido de compra ?
                1 - Sim
                2 - Não
                """,
            NodeValidator = startValidator,
        };

        //Transições de askUserType apenas para satisfazer o teste InitialRegistrationFlowTests
        //Ver depois uma forma de não ficar essa gambiarra
        // askUserType.Transitions["1"] = finish;
        // askUserType.Transitions["2"] = finish;

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

        return new Dictionary<string, IConversationNode>
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