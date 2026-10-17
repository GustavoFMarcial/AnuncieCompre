using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases;

public class UpdateConversationNodePosition(IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    
    public async Task<Result> Handle(UpdateConversationNodePositionInput input, Guid nodeId)
    {
        ConversationNode? node = await conversationNodeRepository.GetByIdAsync(nodeId);

        if (node is null) return Result.Failure("ConversationNode não encontrado");

        node.UpdatePosition(input);

        await unitOfWork.SaveChangesAsync();
        return Result.Success("Position alterado com sucesso");
    }
}