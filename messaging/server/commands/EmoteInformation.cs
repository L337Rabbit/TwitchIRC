using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public enum EmoteSize
    {
        SMALL, MEDIUM, LARGE
    }

    public class EmoteInformation
    {
        public List<EmoteLocation> LocationsInMessage { get; set; }  = new List<EmoteLocation>();
        
        public string EmoteID { get; set; }

        public string GetImageURL(EmoteSize size = EmoteSize.SMALL)
        {
            return EmoteInformation.GetImageURL(EmoteID, size);
        }

        public static string GetImageURL(string emoteID, EmoteSize size = EmoteSize.SMALL)
        {
            switch (size)
            {
                case EmoteSize.SMALL: return "http://static-cdn.jtvnw.net/emoticons/v1/" + emoteID + "/1.0";
                case EmoteSize.MEDIUM: return "http://static-cdn.jtvnw.net/emoticons/v1/" + emoteID + "/2.0";
                case EmoteSize.LARGE: return "http://static-cdn.jtvnw.net/emoticons/v1/" + emoteID + "/3.0";
            }

            return "http://static-cdn.jtvnw.net/emoticons/v1/" + emoteID + "/1.0";
        }
    }
}
