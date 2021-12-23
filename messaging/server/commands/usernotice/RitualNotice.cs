using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public class RitualNotice : UserNoticeMessage
    {
        public RitualType RitualType { get; set; } = RitualType.NEW_CHATTER;

        public String RitualName { get; set; }

        public RitualNotice(string[] parts, Dictionary<string, string> tags) : base(parts, tags, UserNoticeType.RITUAL)
        {
            if(tags.ContainsKey("msg-param-ritual-name"))
            {
                RitualName = tags["msg-param-ritual-name"];

                if(RitualName.Equals("new_chatter"))
                {
                    RitualType = RitualType.NEW_CHATTER;
                }
            }
        }
    }
}
