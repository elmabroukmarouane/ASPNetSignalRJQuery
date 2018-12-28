using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifSignalR.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string TextMessage { get; set; }
    }
}