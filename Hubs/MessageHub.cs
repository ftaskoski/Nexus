using Microsoft.AspNetCore.SignalR;
using Nexus.Models;

namespace Nexus.Hubs
{
    public class MessageHub : Hub
    {
        public async Task JoinChatRoom(string chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
        }

        public async Task LeaveChatRoom(string chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
        }

        public async Task SendMessage(MessageModel message)
        {
            await Clients.Group(message.ChatRoomId).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task StartTyping(string chatRoomId, string username)
        {
            await Clients.OthersInGroup(chatRoomId).SendAsync("UserTyping", username);
        }

        public async Task StopTyping(string chatRoomId, string username)
        {
            await Clients.OthersInGroup(chatRoomId).SendAsync("UserStoppedTyping", username);
        }


    }
}