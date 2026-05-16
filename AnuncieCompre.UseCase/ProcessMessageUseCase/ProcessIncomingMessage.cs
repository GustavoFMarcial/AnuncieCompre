using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.UseCase.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Infra.Providers;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(IUserRepository _userRepository, IDatabase _db, IConversationRepository _conversationRepository, IUnitOfWork _unitOfWork, ConversationFlowProvider _conversationFlowProvider) : IProcessIncomingMessage
{
    private readonly IUserRepository userRepository = _userRepository;
    private readonly IDatabase db = _db;
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly ConversationFlowProvider conversationFlowProvider = _conversationFlowProvider;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    
    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        User? user = await userRepository.GetUserByPhoneAsync(incomingMessage.SenderPhone);
        Conversation? conversation = await conversationRepository.GetConversationByPhoneAsync(incomingMessage.SenderPhone);
        string key = $"session:{incomingMessage.SenderPhone}";
        HashEntry[] session = await db.HashGetAllAsync(key);

        if (conversation is null)
        {
            conversation = Conversation.Create(Phone.Create(incomingMessage.SenderPhone).Value);
            conversationRepository.Add(conversation);
        }

        if (session.Length == 0)
        {
            if (user is null)
            {
                var entries = new HashEntry[]
                {
                    new("phone", incomingMessage.SenderPhone),
                    new("awaitingResponseNodeId", "initial_start"),
                };
                
                await db.HashSetAsync(key, entries);
            }
            else
            {
                if (user.Type.Value == Domain.Enums.UserType.Customer)
                {
                    var entries = new HashEntry[]
                    {
                        new("phone", incomingMessage.SenderPhone),
                        new("awaitingResponseNodeId", "ask_order"),
                    };

                    await db.HashSetAsync(key, entries);
                }
                else if (user.Type.Value == Domain.Enums.UserType.Vendor)
                {
                    var entries = new HashEntry[]
                    {
                        new("phone", incomingMessage.SenderPhone),
                        new("awaitingResponseNodeId", "vendor_registered"),
                    };

                    await db.HashSetAsync(key, entries);
                }
            }

            session = await db.HashGetAllAsync(key);
        }
        
        var data = session.ToDictionary(
            x => x.Name.ToString(),
            x => x.Value.ToString()
        );

        IConversationNode awaitingRespondeNode = conversationFlowProvider.GetById(data["awaitingResponseId"]);

        (ReadOnlyCollection<string> response, string nextStepId) = conversation.HandleMessage(awaitingRespondeNode, incomingMessage.Content, user);

        var hash = new HashEntry[]
        {
            new ("awaitingResponseNodeId", nextStepId),
        };
        await db.HashSetAsync(key, hash);
        await db.KeyExpireAsync(key, TimeSpan.FromMinutes(30));
        await unitOfWork.SaveChangesAsync();

        return response;
    }
}