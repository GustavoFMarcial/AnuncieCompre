using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class UpdateConversationNodePositionRequestExtensions
{
    public static UpdateConversationNodePositionInput ToUpdateConversationNodePositionInput(this UpdateConversationNodePositionRequest request)
    {
        return new UpdateConversationNodePositionInput
        {
            PositionX = request.Positionx,
            PositionY = request.PositionY,
        };
    }
}