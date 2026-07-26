using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Infra.Repositories.MessageRepo;

public interface IMessageRepository : IBaseRepository<Message>
{
    
}