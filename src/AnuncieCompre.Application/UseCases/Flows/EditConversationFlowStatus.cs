using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationFlowStatus(IConversationFlowRepository _conversationFlowRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationFlowRepository conversationFlowRepository = _conversationFlowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid flowId, EditConversationFlowStatusInput input)
    {
    ConversationFlow? flow = await conversationFlowRepository.GetFlowByIdWithNodesAsync(flowId);

        if (flow is null) return Result.Failure("ConversationFlow não encontrado");

        string errors = "";

        foreach (ConversationNode n in flow.Nodes)
        {
            Result nodeResult = n.ValidateTransitions(input.Status);

            if (!nodeResult.IsSuccess)
            {
                errors += $",{nodeResult.Message}";
            }
        }

        Result flowResult = flow.EditStatus(input.Status);

        if (!flowResult.IsSuccess)
        {
            errors += flowResult.Message;
        }
        
        if (errors.Length > 0) return Result.Failure(errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success(flowResult.Message);
    }
}