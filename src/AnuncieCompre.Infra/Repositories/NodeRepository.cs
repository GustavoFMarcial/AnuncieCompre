using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Infra.Data;

namespace AnuncieCompre.Infra.Repositories;

public class NodeRepository(AnuncieCompreContext _context) : BaseRepository<ConversationNode>(_context), INodeRepository
{
    
}