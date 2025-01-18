using Microsoft.AspNetCore.SignalR;

namespace Nexus.Hubs
{
    public class OnlineUsersHub : Hub
    {
        public async Task SendUserStatusChange(string userId, string username, bool isOnline)
        {
            await Clients.All.SendAsync("UserStatusChanged", userId, username, isOnline);
        }
        public async Task SendUserStatusChangeOnLogout(string userId, string username)
        {
            await Clients.All.SendAsync("UserStatusChanged", userId, username, false);
        }
    }
}
