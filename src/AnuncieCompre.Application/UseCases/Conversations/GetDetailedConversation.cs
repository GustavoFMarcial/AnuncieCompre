using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;

namespace AnuncieCompre.Application.UseCases.Conversations;

public class GetDetailedConversation(IConversationRepository _conversationRepository)
{
    private readonly IConversationRepository conversationRepository = _conversationRepository;

    public async Task<Conversation?> Handle(Guid id)
    {
        return await conversationRepository.GetConversationByIdWithMessagesAndUserAsync(id);
    }
}