using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server.membership
{
    public class PartMessage : IRCMessage
    {
        public string User { get; set; }

        public string Channel { get; set; }

        public PartMessage(string[] parts) : base(MessageType.PART)
        {
            //Parse the message
            User = parts[0].Substring(1, parts[0].IndexOf('!') - 1);
            Channel = parts[2].Substring(1);
        }

        public override string ToString()
        {
            return ":" + User + "!" + User + "@" + User + ".tmi.twitch.tv PART #" + Channel;
        }
    }
}
