using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public class ClearSingleMessage : IRCMessage, TaggedMessage, ChannelSpecificMessage
    {
        public string User { get; set; }

        public string Channel { get; set; }

        public string Message { get; set; }

        public string MessageID { get; set; }

        public ClearSingleMessage(string[] parts) : base(MessageType.CLEAR_SINGLE)
        {
            int indexOffset = 0;
            tags = ParseTags(parts[0]);

            if (tags.Count > 0)
            {
                ProcessTags();
                indexOffset++;
            }

            Channel = parts[indexOffset + 2].Substring(1);
            Message = ExtractMessage(parts, indexOffset + 3);
        }

        public void ProcessTags()
        {
            foreach (string key in tags.Keys)
            {
                switch (key)
                {
                    case "login":
                        User = tags["login"];
                        break;
                    case "target-msg-id":
                        MessageID = tags["target-msg-id"];
                        break;
                }
            }
        }

        public override string ToString()
        {
            return ":tmi.twitch.tv CLEARMSG #" + Channel + " :" + Message;
        }
    }
}
