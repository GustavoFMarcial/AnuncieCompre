using AnuncieCompre.Application.Repositories;
using AnuncieCompre.Domain.Aggregates.TransitionAggregate;
using AnuncieCompre.Infra.Data;

namespace AnuncieCompre.Infra.Repositories;

public class ConversationNodeTransitionRepository(AnuncieCompreContext _context) : BaseRepository<ConversationNodeTransition>(_context), IConversationNodeTransitionRepository
{
    
}