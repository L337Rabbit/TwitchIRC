using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public class HostTargetMessage : IRCMessage
    {
        public string HostingChannel { get; set; }

        public string TargetChannel { get; set; }

        public int ViewerCount { get; set; }

        public HostTargetMessage(string[] parts) : base(MessageType.HOST_TARGET)
        {
            //Parse the message
            HostingChannel = parts[2].Substring(1);

            TargetChannel = parts[3].Substring(1);
            if(TargetChannel.Equals("-"))
            {
                TargetChannel = null;
            }

            ViewerCount = int.Parse(parts[4]);
        }

        public override string ToString()
        {
            if (TargetChannel == null)
            {
                return ":tmi.twitch.tv HOSTTARGET #" + HostingChannel + " :" + TargetChannel + " " + ViewerCount;
            }
            else
            {
                return ":tmi.twitch.tv HOSTTARGET #" + HostingChannel + " :- " + ViewerCount;
            }
        }
    }
}
