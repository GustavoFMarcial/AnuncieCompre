namespace AnuncieCompre.Domain.Enums;

public enum ConversationStep
{
    Empty,
    UserNotFullRegistered,
    WaitingToRegisterOrNot,
    WaitingForUserType,
    WaitingForCompanyCategory,
    WaitingForName,
    WaitingForEmail,
    WaitingForCPF,
    WaitingForCNPJ,
    WaitingForOrderOrNot,
    WaitingForProduct,
    WaitingForQuantity,
    WaitingForEndOrNot,
}