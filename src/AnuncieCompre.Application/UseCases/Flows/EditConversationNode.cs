using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationNode(IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result<ConversationNode>> Handle(Guid id, EditConversationNodeInput input)
    {
        ConversationNode? node = await conversationNodeRepository.GetByIdAsync(id);

        if (node is null) return Result<ConversationNode>.Failure("ConversationNode não encontrado");

        Result result = node.Edit(input);

        if (!result.IsSuccess) return Result<ConversationNode>.Failure(result.Message);
        
        await unitOfWork.SaveChangesAsync();
        return Result<ConversationNode>.Success(node, result.Message);
    }
}