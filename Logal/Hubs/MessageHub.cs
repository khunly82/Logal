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
    }
}
