using AnuncieCompre.Domain.Aggregates.MessageAggregate;

namespace AnuncieCompre.Application.Interfaces;

public interface IMessageSender
{
    public Task SendMessageAsync(Message message);
}