using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9.BusinessLogic
{
    public class ChatHub: Hub
    {
        public void startAConversation(string start_receiver, string start_sender)
        {
            Clients.User(start_receiver).sendInvitation(start_receiver, start_sender);
        }

        public void acceptConversation(string accept_receiver, string accept_sender)
        {
         
            Clients.User(accept_receiver).InformedAccept(accept_receiver, accept_sender);

        }

        public void discardConversation(string discard_receiver, string discard_sender) {
            Clients.User(discard_receiver).informedDiscard(discard_receiver, discard_sender);
        }

        public void Send(string receiver, string sender, string message, string time)
        {
            Clients.User(receiver).sendMessage(sender, message,time);
        }
    }
}