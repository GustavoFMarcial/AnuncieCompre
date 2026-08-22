using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class EditConversationNodeTransitionExtensions
{
    public static EditConversationNodeTransitionInput ToEditConversationNodeTransitionInput(this EditConversationNodeTransitionRequest request)
    {
        List<TransitonInput> transitons = request.Transitions.Select(r => new TransitonInput
        {
            Option = r.Option,
            TargetNodeId = r.TargetNodeId,
        }).ToList();

        return new EditConversationNodeTransitionInput
        {
            Transitions = transitons,
        };
    }
}