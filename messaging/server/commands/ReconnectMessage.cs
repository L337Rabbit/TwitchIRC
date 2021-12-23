using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public class ReconnectMessage : IRCMessage
    {
        public ReconnectMessage(string[] parts) : base(MessageType.RECONNECT)
        {
            
        }
    }
}
