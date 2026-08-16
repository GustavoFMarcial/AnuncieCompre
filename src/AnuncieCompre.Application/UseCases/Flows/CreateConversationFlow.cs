using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Infra.Repositories;

namespace AnuncieCompre.Application.UseCases.Flows;

public class CreateConversationFlow(IConversationFlowRepository _flowRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationFlowRepository flowRepository = _flowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result<ConversationFlow>> Handle(CreateConversationFlowInput request)
    {
        Result<ConversationFlow> result = ConversationFlow.Create(request.Name, request.Description, request.Status);

        if (!result.IsSuccess) return result;

        flowRepository.Add(result.Value);
        await unitOfWork.SaveChangesAsync();

        return result;
    }
}