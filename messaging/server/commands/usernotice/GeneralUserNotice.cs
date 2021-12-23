using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public class GeneralUserNotice : UserNoticeMessage
    {
        public GeneralUserNotice(string[] parts, Dictionary<string, string> tags) : base(parts, tags, UserNoticeType.GENERAL)
        {
            Console.WriteLine("Created a GENERAL USER NOTICE: " + tags);
        }
    }
}
