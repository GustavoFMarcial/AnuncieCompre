using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.UseCase.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(IConversationRepository _conversationRepository, IUserRepository _userRepository, IUnitOfWork _unitOfWork) : IProcessIncomingMessage
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IUserRepository userRepository = _userRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    
    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        Conversation? conversation = await conversationRepository.GetConversationByPhoneAsync(incomingMessage.SenderPhone);
        User? user = await userRepository.GetUserByPhoneAsync(incomingMessage.SenderPhone);

        if (conversation is null || user is null)
        {
            conversation = Conversation.Create(VOPhone.Create(incomingMessage.SenderPhone).Value);
            user = User.Create(VOPhone.Create(incomingMessage.SenderPhone).Value);
            conversationRepository.Add(conversation);
            userRepository.Add(user);
        }

        ReadOnlyCollection<string> response = conversation.HandleMessage(incomingMessage, user);

        await unitOfWork.SaveChangesAsync();

        return response;
    }
}