using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public class RoomStateMessage : IRCMessage, TaggedMessage, ChannelSpecificMessage
    {
        public string Channel { get; set; }

        public bool EmoteOnlyMode { get; set; } = false;

        public long RoomID { get; set; }

        public bool FollowersOnlyMode { get; set; } = false;

        public bool R9KMode { get; set; } = false;

        public int MessageWaitTime { get; set; }

        public bool SubOnlyMode { get; set; } = false;

        public RoomStateMessage(string[] parts) : base(MessageType.ROOM_STATE)
        {
            int indexOffset = 0;
            tags = ParseTags(parts[0]);

            if (tags.Count > 0)
            {
                ProcessTags();
                indexOffset++;
            }

            Channel = parts[indexOffset + 2].Substring(1);
        }

        public override string ToString()
        {
            return ":tmi.twitch.tv ROOMSTATE #" + Channel;
        }

        public void ProcessTags()
        {
            foreach (string key in tags.Keys)
            {
                switch (key)
                {
                    case "emote-only":
                        EmoteOnlyMode = !tags["emote-only"].Equals("0");
                        break;
                    case "followers-only":
                        FollowersOnlyMode = !!tags["followers-only"].Equals("0");
                        break;
                    case "r9k":
                        R9KMode = !tags["r9k"].Equals("0");
                        break;
                    case "room-id":
                        this.RoomID = long.Parse(tags["room-id"]);
                        break;
                    case "slow":
                        MessageWaitTime = int.Parse(tags["slow"]);
                        break;
                    case "subs-only":
                        SubOnlyMode = !tags["subs-only"].Equals("0");
                        break;
                }
            }
        }
    }
}
