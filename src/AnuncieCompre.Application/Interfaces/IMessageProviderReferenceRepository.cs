using AnuncieCompre.Domain.Aggregates.MessageProviderReferenceAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.Interfaces;

public interface IMessageProviderReferenceRepository : IBaseRepository<MessageProviderReference>
{
    public Task<MessageProviderReference?> GetMessageProviderReferenceWithMessageByMessageProviderSidAsync(string messageSid);
}