using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationFlowStatus(IConversationFlowRepository _conversationFlowRepository, IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationFlowRepository conversationFlowRepository = _conversationFlowRepository;
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid flowId, EditConversationFlowStatusInput input)
    {
        ConversationFlow? flow = await conversationFlowRepository.GetByIdAsync(flowId);

        if (flow is null) return Result.Failure("ConversationFlow não encontrado");

        List<ConversationNode> nodes = await conversationNodeRepository.GetConversationNodesByFlowIdAsync(flow.Id);
        string errors = "";

        foreach (ConversationNode n in nodes)
        {
            Result nodeResult = n.ValidateTransitions(input);

            if (!nodeResult.IsSuccess)
            {
                errors += $",{nodeResult.Message}";
            }
        }

        Result flowResult = flow.EditStatus(input, nodes);

        if (!flowResult.IsSuccess)
        {
            errors += flowResult.Message;
        }
        
        if (errors.Length > 0) return Result.Failure(errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success(flowResult.Message);
    }
}