using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Aggregates.MessageProviderReferenceAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.Interfaces;

public interface IMessageSender
{
    public Task<Result<MessageProviderReference>> SendMessageAsync(Message message);
}