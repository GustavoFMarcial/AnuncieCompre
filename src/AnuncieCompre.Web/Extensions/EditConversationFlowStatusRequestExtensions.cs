using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class EditConversationFlowStatusRequestExtensions
{
    public static EditConversationFlowStatusInput ToEditConversationFlowStatusInput(this EditConversationFlowStatusRequest request)
    {
        return new EditConversationFlowStatusInput
        {
            Status = request.Status.ToLower() switch
            {
                "published" => FlowStatus.Published,
                "draft" => FlowStatus.Draft,
                _ => FlowStatus.Draft,
            }
        };
    }
}