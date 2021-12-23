using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.client
{
    public class Viewer
    {
        public long UserID { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string NameColor { get; set; }

        public string[] EmoteSets { get; set; }
    }
}
