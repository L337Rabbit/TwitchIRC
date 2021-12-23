using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging
{
    public interface TaggedMessage
    {
        void ProcessTags();
    }
}
