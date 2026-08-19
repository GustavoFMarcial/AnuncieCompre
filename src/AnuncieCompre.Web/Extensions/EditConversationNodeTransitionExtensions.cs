using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class EditConversationNodeTransitionExtensions
{
    public static List<EditConversationNodeTransitionInput> ToEditConversationNodeTransitionInput(this List<EditConversationNodeTransitionRequest> request)
    {
        List<EditConversationNodeTransitionInput> input = request.Select(r => new EditConversationNodeTransitionInput
        {
            Option = r.Option,
            TargetNodeId = r.TargetNodeId,
        }).ToList();

        return input;
    }
}