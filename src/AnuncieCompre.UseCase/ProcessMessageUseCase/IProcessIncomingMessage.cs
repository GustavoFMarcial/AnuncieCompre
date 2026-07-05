using System.Collections.ObjectModel;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.UseCase.ProcessMessageUseCase;

public interface IProcessIncomingMessage
{
    public Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage);
}