using Logal.Forms;
using Microsoft.AspNetCore.SignalR;

namespace Logal.Hubs
{
    public class MessageHub : Hub
    {

        public void Send(MessageForm form)
        {
            Clients.Others.SendAsync("newMessage", form.Message);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
