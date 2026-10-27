using Microsoft.AspNetCore.SignalR;

namespace AnuncieCompre.Web.Hubs;

public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "global");

        await base.OnConnectedAsync();
    }
    public async Task JoinConversation(Guid conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
    }

    public async Task LeaveConversation(Guid conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
    }
}