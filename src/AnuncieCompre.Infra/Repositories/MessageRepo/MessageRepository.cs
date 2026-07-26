using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Infra.Data;

namespace AnuncieCompre.Infra.Repositories.MessageRepo;

public class MessageRepository(AnuncieCompreContext _context) : BaseRepository<Message>(_context), IMessageRepository
{
    
}