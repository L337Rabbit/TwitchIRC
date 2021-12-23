using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public class ClearAllMessage : IRCMessage, ChannelSpecificMessage
    {
        public string User { get; set; }

        public string Channel { get; set; }

        public ClearAllMessage(string[] parts) : base(MessageType.CLEAR_ALL)
        {
            int indexOffset = 0;
            tags = ParseTags(parts[0]);

            if (tags.Count > 0)
            {
                indexOffset++;
            }

            Channel = parts[indexOffset + 2].Substring(1);
            User = parts[indexOffset + 3].Substring(1);
        }

        public override string ToString()
        {
             return ":tmi.twitch.tv CLEARCHAT #" + Channel + " :" + User;
        }
    }
}
