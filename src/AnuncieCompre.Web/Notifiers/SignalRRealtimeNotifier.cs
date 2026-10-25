using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.Extensions;
using AnuncieCompre.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AnuncieCompre.Infra.Notifiers;

public class SignalRRealtimeNotifier(IHubContext<ChatHub> hubContext) : IRealtimeNotifier
{
    public async Task NotifyNewMessageAsync(Guid conversationId, Message message)
    {
        RealTimeMessageDTO messageDTO = message.ToMessageDTO();
        await hubContext.Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", messageDTO);
    }
}