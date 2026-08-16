using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Infra.Data;

namespace AnuncieCompre.Infra.Repositories;

public class ConversationNodeRepository(AnuncieCompreContext _context) : BaseRepository<ConversationNode>(_context), IConversationNodeRepository
{
    
}