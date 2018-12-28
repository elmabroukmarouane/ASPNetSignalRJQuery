using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using NotifSignalR.Models;

namespace NotifSignalR
{
    public class NotificationsHub : Hub
    {
        static List<Utilisateur> connectedUsers = new List<Utilisateur>();
        public override Task OnConnected()
        {
            string id = Context.ConnectionId;
            if (connectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                int countNext = 0;
                countNext = connectedUsers.Count + 1;
                Utilisateur utilisateur = new Utilisateur { ConnectionId = id, UserName = "User " + countNext };
                connectedUsers.Add(utilisateur);
                Clients.All.onNewUserConnected(utilisateur, connectedUsers);
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string id = Context.ConnectionId;
            Utilisateur utilisateur = connectedUsers.Find(x => x.ConnectionId == id);
            connectedUsers.Remove(utilisateur);
            Clients.Others.onDisconnectUser(connectedUsers);
            return base.OnDisconnected(stopCalled);
        }

        public void SendNotificationAll(string message, string username)
        {
            Utilisateur utilisateur = connectedUsers.Find(x => x.UserName == username);
            Clients.Others.incrementcounterNotifications();
            Clients.All.broadCastMessages(message, utilisateur);
        }

        public void SendNotificationSelected(string message, string connectionId, string connectionIdMoi)
        {
            Utilisateur utilisateur = connectedUsers.Find(x => x.ConnectionId == connectionIdMoi);
            Clients.Client(connectionId).incrementcounterNotifications();
            Clients.Client(connectionIdMoi).sendPrivateMessage(message, utilisateur);
            Clients.Client(connectionId).sendPrivateMessage(message, utilisateur);
        }
    }
}