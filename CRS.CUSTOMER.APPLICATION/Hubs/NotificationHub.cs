using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace CRS.CUSTOMER.APPLICATION.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}