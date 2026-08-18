using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.UseCases;

public class CreateConversationNode(IConversationFlowRepository _conversationFlowRepository ,IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationFlowRepository conversationFlowRepository = _conversationFlowRepository;
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result<ConversationNode>> Handle(Guid id)
    {
        ConversationFlow? flow = await conversationFlowRepository.GetByIdAsync(id);

        if (flow is null) return Result<ConversationNode>.Failure("ConversationFlow não encontrado");

        Result<ConversationNode> result = ConversationNode.Create(flow);

        if (!result.IsSuccess) return Result<ConversationNode>.Failure(result.Message);

        conversationNodeRepository.Add(result.Value);
        await unitOfWork.SaveChangesAsync();
        return Result<ConversationNode>.Success(result.Value, "ConversationNode criado com sucesso");
    }
}