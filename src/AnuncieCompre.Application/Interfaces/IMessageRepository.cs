using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.Interfaces;

public interface IMessageRepository : IBaseRepository<Message>
{
    
}