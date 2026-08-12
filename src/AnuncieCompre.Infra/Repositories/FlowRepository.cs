using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Infra.Data;

namespace AnuncieCompre.Infra.Repositories;

public class FlowRepository(AnuncieCompreContext _context) : BaseRepository<ConversationFlow>(_context), IFlowRepository
{
    
}