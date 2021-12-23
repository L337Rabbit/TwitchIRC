using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public class RaidNotice : UserNoticeMessage
    {
        public String RaiderDisplayName { get; set; }

        public String RaiderUsername { get; set; }

        public int ViewerCount { get; set; }

        public RaidNotice(string[] parts, Dictionary<string, string> tags) : base(parts, tags, UserNoticeType.RAID)
        {
            ProcessTags();
        }

        public new void ProcessTags()
        {
            foreach (string key in tags.Keys)
            {
                switch (key)
                {
                    case "msg-param-displayName":
                        RaiderDisplayName = tags["msg-param-displayName"];
                        break;
                    case "msg-param-login":
                        RaiderUsername = tags["msg-param-login"];
                        break;
                    case "msg-param-viewerCount":
                        ViewerCount = int.Parse(tags["msg-param-viewerCount"]);
                        break;
                }
            }
        }
    }
}
