using System.Collections.ObjectModel;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases.ProcessMessageUseCase;

public interface IProcessIncomingMessage
{
    public Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage);
}