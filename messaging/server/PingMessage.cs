using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server
{
    public class PingMessage : IRCMessage
    {
        public string Origin { get; set; }

        public PingMessage(string message) : base(MessageType.PING)
        {
            //Parse the message
            this.Origin = message.Substring(message.IndexOf(':') + 1);
            MessageType = MessageType.PING;
        }

        public override string ToString()
        {
            return "PING :" + Origin;
        }
    }
}
