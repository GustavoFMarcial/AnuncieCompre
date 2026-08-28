using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Application.UseCases.Conversations;

public class GetConversations(IConversationRepository _conversationRepository)
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;

    public async Task<List<Conversation>> Handle(ConversationStatus? status)
    {
        return await conversationRepository.GetConversationsByStatusToListAsync(status);
    }
}