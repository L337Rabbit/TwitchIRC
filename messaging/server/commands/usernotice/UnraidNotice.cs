using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public class UnraidNotice : UserNoticeMessage
    {
        public UnraidNotice(string[] parts, Dictionary<string, string> tags) : base(parts, tags, UserNoticeType.UNRAID)
        {

        }
    }
}
