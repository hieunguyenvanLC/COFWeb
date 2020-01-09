using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace COF.API.SignalR
{
    public class OrderNotificationHub : Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public void BroadcastMessage(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

        public void SendUser(string name, string userId, string message)
        {
            Clients.User(userId).echo(name, message);
        }

        public void SendUsers(string name, IList<string> userIds, string message)
        {
            Clients.Users(userIds).echo(name, message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}