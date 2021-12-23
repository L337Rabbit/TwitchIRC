using okitoki.twitch.irc.messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.client
{
    public class PongMessage : IRCMessage
    {
        public string Destination { get; set; }

        public PongMessage() : base(MessageType.PONG)
        {
            this.Destination = "tmi.twitch.tv";

            MessageType = MessageType.PONG;
        }

        public override string ToString()
        {
            return "PONG :" + Destination;
        }
    }
}
