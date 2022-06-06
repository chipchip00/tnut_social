using System;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            
            // Call the addNewMessageToPage method to update clients.
            await Clients.All.addNewMessageToPage(message);
        }
    }
}