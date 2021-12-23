using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public class GlobalUserStateMessage : UserStateMessage, TaggedMessage
    {
        public long UserID { get; set; }

        public string UserType { get; set; }

        public GlobalUserStateMessage(string[] parts) : base(parts, MessageType.GLOBAL_USER_STATE)
        {
            ProcessTags();
        }

        public new void ProcessTags()
        {
            foreach (string key in tags.Keys)
            {
                switch (key)
                {
                    case "user-id":
                        UserID = long.Parse(tags["user-id"]);
                        break;
                    case "user-type":
                        UserType = tags["user-type"];
                        break;
                }
            }
        }

        public override string ToString()
        {
            return ":tmi.twitch.tv GLOBALUSERSTATE";
        }
    }
}
