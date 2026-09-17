using System.Text.Json;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.Services;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Aggregates.MessageProviderReferenceAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;
using StackExchange.Redis;

namespace AnuncieCompre.Application.UseCases.ProcessMessageUseCase;

public class ProcessMessageStatus(
    IMessageProviderReferenceRepository _messageProviderReferenceRepository, 
    MessageFailureHandler _messageFailureHandler, 
    IDatabase _db, 
    IUnitOfWork _unitOfWork)
{
    private readonly IMessageProviderReferenceRepository messageProviderReferenceRepository = _messageProviderReferenceRepository;
    private readonly MessageFailureHandler messageFailureHandler = _messageFailureHandler;
    private readonly IDatabase db = _db;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    public async Task<Result> Handle(TwilioStatusCallbackInput input)
    {
        MessageProviderReference? messageProvider = await messageProviderReferenceRepository.GetMessageProviderReferenceWithMessageByMessageProviderSidAsync(input.MessageSid);

        if (messageProvider is null)
        {
            return Result.Failure("Mensagem não encontrada");
        }

        Message message = messageProvider.Message;
        message.SetMessageStatus(input.MessageStatus);

        if (input.MessageStatus is MessageStatus.Failed || input.MessageStatus is MessageStatus.Undelivered)
        {
            await messageFailureHandler.Handle(input.ErrorCode, message);
        }

        await unitOfWork.SaveChangesAsync();
        return Result.Success("Mensagem encontrada");
    }
}