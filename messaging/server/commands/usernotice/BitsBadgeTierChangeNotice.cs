using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public class BitsBadgeTierChangeNotice : UserNoticeMessage
    {
        public int BitBadgeNumberEarned { get; set; }

        public BitsBadgeTierChangeNotice(string[] parts, Dictionary<string, string> tags) : base(parts, tags, UserNoticeType.BITS_BADGE_TIER)
        {
            if(tags.ContainsKey("msg-param-threshold"))
            {
                BitBadgeNumberEarned = int.Parse(tags["msg-param-threshold"]);
            }
        }
    }
}
