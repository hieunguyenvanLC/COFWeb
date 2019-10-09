using COF.BusinessLogic.Models.Order;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.API.Hubs
{
    [HubName("messageHub")]
    public class MessageHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();
        //public static void PushToAllUsers(AnnouncementViewModel message, MessageHub hub)
        //{
        //    IHubConnectionContext<dynamic> clients = GetClients(hub);
        //    clients.All.addAnnouncement(message);
        //}
        /// <summary>
        /// Push to a specific user
        /// </summary>
        /// <param name="who"></param>
        /// <param name="message"></param>
        public static void PushOrderToUser(string who, OrderCreateModel message, MessageHub hub)
        {
            IHubConnectionContext<dynamic> clients = GetClients(hub);
            foreach (var connectionId in _connections.GetConnections(who))
            {
                clients.Client(connectionId).notifyOrder(message);
            }
        }

        ///// <summary>
        ///// Push to list users
        ///// </summary>
        ///// <param name="who"></param>
        ///// <param name="message"></param>
        //public static void PushToUsers(string[] whos, AnnouncementViewModel message, MessageHub hub)
        //{
        //    IHubConnectionContext<dynamic> clients = GetClients(hub);
        //    for (int i = 0; i < whos.Length; i++)
        //    {
        //        var who = whos[i];
        //        foreach (var connectionId in _connections.GetConnections(who))
        //        {
        //            clients.Client(connectionId).addChatMessage(message);
        //        }
        //    }

        //}
        private static IHubConnectionContext<dynamic> GetClients(MessageHub messageHub)
        {
            if (messageHub == null)
                return GlobalHost.ConnectionManager.GetHubContext<MessageHub>().Clients;
            else
                return messageHub.Clients;
        }

        /// <summary>
        /// Connect user to hub
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            _connections.Add(Context.User.Identity.Name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _connections.Remove(Context.User.Identity.Name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            if (!_connections.GetConnections(Context.User.Identity.Name).Contains(Context.ConnectionId))
            {
                _connections.Add(Context.User.Identity.Name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}
