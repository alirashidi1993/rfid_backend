using Microsoft.AspNetCore.SignalR;

namespace Sgms.Backend.Hubs
{
    public class CardHub:Hub
    {
        public async Task GrantAccess(string token) =>
            await Clients.All.SendAsync("AccessGranted", token);
    }
}
