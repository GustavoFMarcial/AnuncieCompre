using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.UseCases;

public class DeleteConversationNode(IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid nodeId)
    {
        ConversationNode? node = await conversationNodeRepository.GetByIdAsync(nodeId);

        if (node is null) return Result.Failure("ConversationNode não encontrado");

        conversationNodeRepository.Delete(node);
        List<ConversationNode> nodes = await conversationNodeRepository.GetConversationNodeByTransitionTargetNodeIdAsync(nodeId);

        foreach (ConversationNode n in nodes)
        {
            n.RemoveTransition(nodeId);
        }

        await unitOfWork.SaveChangesAsync();
        return Result.Success("ConversationNode deletado com sucesso");
    }
}