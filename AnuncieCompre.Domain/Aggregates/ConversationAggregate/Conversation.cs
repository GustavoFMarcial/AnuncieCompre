using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public VOPhone UserPhone { get; private set; } = default!;
    public ConversationStep CurrentStep { get; private set; } = ConversationStep.UserNotFullRegistered;
    public ConversationTempData TempData { get; private set; } = new();

    private Conversation() { }

    private Conversation(VOPhone phone)
    {
        UserPhone = phone;
    }

    public static Conversation Create(VOPhone userPhone)
    {
        var conversation = new Conversation(userPhone);
        return conversation;
    }

    public ReadOnlyCollection<string> HandleMessage(IncomingMessageRequest message, User user)
    {
        return CurrentStep switch
        {
            ConversationStep.Empty => HandleEmptyStep(user),
            ConversationStep.UserNotFullRegistered => HandleUserNotFullRegisteredStep(),
            ConversationStep.WaitingToRegisterOrNot => HandleWaitingToRegisterOrNotStep(message.Content),
            ConversationStep.WaitingForUserType => HandleWaitingForUserTypeStep(message.Content),
            ConversationStep.WaitingForName => HandleWaitingForNameStep(message.Content),
            ConversationStep.WaitingForCompanyCategory => HandleWaitingForCompanyCategoryStep(message.Content, user),
            ConversationStep.WaitingForEmail => HandleWaitingForEmailStep(message.Content, user),
            ConversationStep.WaitingForCPF => HandleWaitingForCPFStep(message.Content, user),
            ConversationStep.WaitingForCNPJ => HandleWaitingForCNPJStep(message.Content, user),
            ConversationStep.WaitingForOrderOrNot => HandleWaitingForOrderOrNotStep(message.Content),
            ConversationStep.WaitingForProduct => HandleWaitingForProductStep(message.Content),
            ConversationStep.WaitingForQuantity => HandleWaitingForQuantityStep(message.Content),
            ConversationStep.WaitingForEndOrNot => HandleWaitingForEndOrNotStep(message.Content),
            _ => new List<string> { "Não compreendi sua mensagem, envie novamente." }.AsReadOnly(),
        };
    }

    // Usuário já registrado volta a conversar
    private ReadOnlyCollection<string> HandleEmptyStep(User user)
    {
        if (user.Type == UserType.Customer)
        {
            CurrentStep = ConversationStep.WaitingForOrderOrNot;

            return new List<string>
            {
                $"""
                Olá, bem-vindo novamente {user.Name!.FullName}! Deseja criar um pedido novo?

                1 - Sim
                2 - Não
                """
            }.AsReadOnly();
        }

        if (user.Type == UserType.Vendor)
        {
            return new List<string>
            {
                $"""
                Olá, bem-vindo novamente {user.Name!.FullName}! 
                
                Não implementado ainda funcionalidades para fornecedores, volte em breve.
                """
            }.AsReadOnly();
        }

        return new List<string> { }.AsReadOnly();
    }

    // Primeiro contato — usuário ainda não registrado
    private ReadOnlyCollection<string> HandleUserNotFullRegisteredStep()
    {
        CurrentStep = ConversationStep.WaitingToRegisterOrNot;

        return new List<string>
        {
            """
            Olá, bem-vindo! Vimos que é novo por aqui.

            Deseja se registrar no AnuncieCompre para utilizar nosso sistema?
            1 - Sim
            2 - Não
            """
        }.AsReadOnly();
    }

    // Quer se registrar?
    private ReadOnlyCollection<string> HandleWaitingToRegisterOrNotStep(string message)
    {
        if (message == "1")
        {
            CurrentStep = ConversationStep.WaitingForUserType;
            return new List<string>
            {
                """
                Você deseja usar nosso sistema como cliente ou como fornecedor?

                1 - Cliente
                2 - Fornecedor
                """
            }.AsReadOnly();
        }

        if (message == "2")
        {
            CurrentStep = ConversationStep.UserNotFullRegistered;
            return new List<string> { "Ok, até logo!" }.AsReadOnly();
        }

        return new List<string> { "Opção inválida, envie novamente." }.AsReadOnly();
    }

    // Descobre o tipo primeiro — personaliza todas as perguntas seguintes
    // Customer: → Nome completo
    // Vendor:   → Ramo da empresa
    private ReadOnlyCollection<string> HandleWaitingForUserTypeStep(string message)
    {
        if (message == "1")
        {
            TempData.SetUserType(UserType.Customer);
            CurrentStep = ConversationStep.WaitingForName;
            return new List<string> { "Qual seu nome completo?" }.AsReadOnly();
        }

        if (message == "2")
        {
            TempData.SetUserType(UserType.Vendor);
            CurrentStep = ConversationStep.WaitingForCompanyCategory;
            return new List<string>
            {
                 $"""
                Qual o ramo da empresa?

                {CompanyCategoryExtensions.PrintNames()}
                """
            }.AsReadOnly();
        }

        return new List<string> { "Opção inválida, envie novamente." }.AsReadOnly();
    }

    // Coleta nome — pergunta já foi personalizada pelo tipo
    // Customer: Nome completo → Email
    // Vendor:   Nome da empresa → Email
    private ReadOnlyCollection<string> HandleWaitingForNameStep(string name)
    {
        Result<VOName> result = VOName.Create(name);

        if (result.IsSuccess)
        {
            TempData.SetName(result.Value);
            CurrentStep = ConversationStep.WaitingForEmail;
            return new List<string> { "Qual email para cadastro?" }.AsReadOnly();
        }

        return new List<string> { $"{result.Message}, envie novamente." }.AsReadOnly();
    }

    // Coleta categoria
    // Customer → Product
    // Vendor → Name
    private ReadOnlyCollection<string> HandleWaitingForCompanyCategoryStep(string message, User user)
    {
        if (!int.TryParse(message, out int category))
            return new List<string> { "Opção inválida, envie novamente." }.AsReadOnly();

        if (category == 0)
        {
            CurrentStep = ConversationStep.UserNotFullRegistered;
            return new List<string>
            {
                """
                Sinto muito, ainda não temos essa categoria. Iremos estudar a possibilidade de adicioná-la.

                Volte depois para checar.
                """
            }.AsReadOnly();
        }

        if (category > 0 && category <= CompanyCategoryExtensions.Lenght())
        {
            if (user.Type == UserType.Customer)
            {
                TempData.SetCompanyCategory((CompanyCategory)category);
                CurrentStep = ConversationStep.WaitingForProduct;
                return new List<string> { "Qual produto deseja comprar?" }.AsReadOnly();
            }

            if (user.Type == UserType.Vendor)
            {
                TempData.SetCompanyCategory((CompanyCategory)category);
                CurrentStep = ConversationStep.WaitingForName;
                return new List<string> { "Qual o nome da empresa?" }.AsReadOnly();
            }
        }

        return new List<string> { "Opção inválida, envie novamente." }.AsReadOnly();
    }

    // Coleta email
    // Customer: Email → CPF
    // Vendor:   Email → CNPJ
    private ReadOnlyCollection<string> HandleWaitingForEmailStep(string email, User user)
    {
        Result<VOEmail> result = VOEmail.Create(email);

        if (result.IsSuccess)
        {
            TempData.SetEmail(result.Value);
            var domainEvent = new UserSentDataToRegisterDomainEvent(user.Id, TempData.Name!, TempData.Email!, TempData.UserType);
            AddDomainEvent(domainEvent);

            if (TempData.UserType == UserType.Customer)
            {
                CurrentStep = ConversationStep.WaitingForCPF;
                return new List<string> { "Qual seu CPF?" }.AsReadOnly();
            }
            else
            {
                CurrentStep = ConversationStep.WaitingForCNPJ;
                return new List<string> { "Qual o CNPJ da empresa?" }.AsReadOnly();
            }
        }

        return new List<string> { $"{result.Message}, envie novamente." }.AsReadOnly();
    }

    // Coleta CPF — finaliza registro de Customer
    private ReadOnlyCollection<string> HandleWaitingForCPFStep(string document, User user)
    {
        Result<VOCPF> result = VOCPF.Create(document);

        if (result.IsSuccess)
        {
            TempData.SetCPF(result.Value);

            var domainEvent = new CustomerSentDataToRegisterDomainEvent(user, TempData.CPF!);
            AddDomainEvent(domainEvent);
            TempData.Clear();

            CurrentStep = ConversationStep.WaitingForOrderOrNot;
            return new List<string>
            {
                """
                Obrigado por se registrar no AnuncieCompre!

                Deseja já realizar um pedido?
                1 - Sim
                2 - Não
                """
            }.AsReadOnly();
        }

        return new List<string> { $"{result.Message}, envie novamente." }.AsReadOnly();
    }

    // Coleta CNPJ — finaliza registro de Vendor
    private ReadOnlyCollection<string> HandleWaitingForCNPJStep(string document, User user)
    {
        Result<VOCNPJ> result = VOCNPJ.Create(document);

        if (result.IsSuccess)
        {
            TempData.SetCNPJ(result.Value);

            var domainEvent = new VendorSentDataToRegisterDomainEvent(user, TempData.Category, TempData.CNPJ!);
            AddDomainEvent(domainEvent);
            TempData.Clear();

            CurrentStep = ConversationStep.Empty;
            return new List<string>
            {
                """
                Obrigado por se registrar no AnuncieCompre!

                Assim que pedidos compatíveis com sua categoria aparecerem você será notificado.
                """
            }.AsReadOnly();
        }

        return new List<string> { $"{result.Message}, envie novamente." }.AsReadOnly();
    }

    // Deseja fazer um pedido?
    private ReadOnlyCollection<string> HandleWaitingForOrderOrNotStep(string message)
    {
        if (message == "1")
        {
            CurrentStep = ConversationStep.WaitingForCompanyCategory;
            return new List<string>
            {
                $"""
                Qual categoria de produto deseja comprar?

                {CompanyCategoryExtensions.PrintNames()}
                """
            }.AsReadOnly();
        }

        if (message == "2")
        {
            CurrentStep = ConversationStep.Empty;
            return new List<string> { "Ok, até logo!" }.AsReadOnly();
        }

        return new List<string> { "Não compreendi sua mensagem, envie novamente." }.AsReadOnly();
    }

    // Coleta produto
    private ReadOnlyCollection<string> HandleWaitingForProductStep(string message)
    {
        Result<VOProduct> result = VOProduct.Create(message);

        if (result.IsSuccess)
        {
            TempData.SetProduct(result.Value);
            CurrentStep = ConversationStep.WaitingForQuantity;
            return new List<string> { "Qual quantia deseja comprar?" }.AsReadOnly();
        }

        return new List<string> { $"{result.Message}, envie novamente." }.AsReadOnly();
    }

    // Coleta quantidade — finaliza pedido
    private ReadOnlyCollection<string> HandleWaitingForQuantityStep(string message)
    {
        Result<VOQuantity> result = VOQuantity.Create(message);

        if (result.IsSuccess)
        {
            TempData.SetQuantity(result.Value);

            var domainEvent = new CustomerSentDataToOrderDomainEvent(UserPhone, TempData.Product!, TempData.Quantity!, TempData.Category);
            AddDomainEvent(domainEvent);
            TempData.Clear();

            CurrentStep = ConversationStep.WaitingForEndOrNot;
            return new List<string>
            {
                """
                Pedido criado com sucesso!

                Deseja criar outro pedido?
                1 - Sim
                2 - Não
                """
            }.AsReadOnly();
        }

        return new List<string> { $"{result.Message}, envie novamente." }.AsReadOnly();
    }

    // Deseja criar outro pedido?
    private ReadOnlyCollection<string> HandleWaitingForEndOrNotStep(string message)
    {
        if (message == "1")
        {
            CurrentStep = ConversationStep.WaitingForCompanyCategory;
            return new List<string>
            {
                $"""
                Qual categoria de produto deseja comprar?

                {CompanyCategoryExtensions.PrintNames()}
                """
            }.AsReadOnly();
        }

        if (message == "2")
        {
            CurrentStep = ConversationStep.Empty;
            return new List<string> { "Ok, até logo!" }.AsReadOnly();
        }

        return new List<string> { "Não compreendi sua mensagem, envie novamente." }.AsReadOnly();
    }
}