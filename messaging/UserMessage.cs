using okitoki.twitch.irc.messaging.server.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.client;

namespace okitoki.twitch.irc.messaging
{
    public abstract class UserMessage : IRCMessage, TaggedMessage, ChannelSpecificMessage
    {
        public string Username { get; set; }

        public string Channel { get; set; }

        public string NameColor { get; set; }

        public string DisplayName { get; set; }

        public bool Moderator { get; set; } = false;

        public int MonthsSubscribed { get; set; }

        public List<EmoteInformation> EmoteInfo { get; set; }

        public List<Badge> Badges { get; set; }

        protected UserMessage(string[] parts, Dictionary<string, string> tags, MessageType messageType) : base(messageType)
        {
            this.tags = tags;
            int indexOffset = 0;

            if (tags.Count > 0)
            {
                ProcessTags();
                indexOffset++;
            }
            if (parts[indexOffset].Contains('!'))
            {
                Username = parts[indexOffset].Substring(1, parts[indexOffset].IndexOf('!') - 1);
            } 
            else if(DisplayName != null)
            {
                Username = DisplayName.ToLower();
            }

            if (parts.Length > indexOffset + 2)
            {
                Channel = parts[indexOffset + 2].Substring(1);
            }
        }

        protected UserMessage(string[] parts, MessageType messageType) : this(parts, ParseTags(parts[0]), messageType) { }

        public void ProcessTags()
        {
            foreach(string key in tags.Keys) {
                switch (key)
                {
                    case "badges":
                        ProcessBadges();
                        break;
                    case "badge-info":
                        ProcessBadgeInfo();
                        break;
                    case "color":
                        NameColor = tags["color"]; ;
                        break;
                    case "display-name":
                        DisplayName = tags["display-name"]; ;
                        break;
                    case "emotes":
                        ProcessEmoteInformation();
                        break;
                    case "mod":
                        Moderator = !tags["mod"].Equals("0"); ;
                        break;
                }
            }
        }

        protected void ProcessBadges()
        {
            Badges = new List<Badge>();
            string badgeString = tags["badges"];
            string[] badges = badgeString.Split(',');
            for (int i = 0; i < badges.Length; i++)
            {
                //TODO Create a new badge based on the type.
                string badgeTypeString = badges[i];
                string version = null;
                if(badgeTypeString.Contains('/'))
                {
                    int slashIdx = badgeTypeString.IndexOf('/');
                    version = badgeTypeString.Substring(slashIdx + 1);
                    badgeTypeString = badgeTypeString.Substring(0, slashIdx);
                }
                BadgeType badgeType = BadgeType.UNKNOWN;
                if (Enum.TryParse<BadgeType>(badgeTypeString, out badgeType))
                {
                    Badge badge = new Badge();
                    badge.BadgeType = badgeType;
                    badge.Version = version;
                    Badges.Add(badge);
                }
            }
        }

        protected void ProcessBadgeInfo()
        {
            string badgeInfoString = tags["badge-info"];

            //TODO Set badge values from metadata
            string[] infoParts = badgeInfoString.Split(',');
            Dictionary<string, string> badgeInfo = new Dictionary<string, string>();

            for (int i = 0; i < infoParts.Length; i++)
            {
                if (infoParts[i].Contains('/'))
                {
                    string[] keyValue = infoParts[i].Split('/');
                    badgeInfo.Add(keyValue[0], keyValue[1]);
                }
            }

            if (badgeInfo.ContainsKey("subscriber"))
            {
                MonthsSubscribed = int.Parse(badgeInfo["subscriber"]);
            }
        }

        protected void ProcessEmoteInformation()
        {
            List<EmoteInformation> emoteInfos = new List<EmoteInformation>();
            string currentEmoteValue;
            string emoteString = tags["emotes"];

            for (int i = 0; i < emoteString.Length; i++)
            {
                currentEmoteValue = "";

                //Read the emote ID
                while (i < emoteString.Length && emoteString[i] != ':')
                {
                    currentEmoteValue += emoteString[i];
                    i++;
                }

                ///Create a new EmoteInformation object to store emote location info
                EmoteInformation emoteInfo = new EmoteInformation();
                emoteInfo.EmoteID = currentEmoteValue;

                //Move past the colon
                i++;

                List<EmoteLocation> locations = new List<EmoteLocation>();
                bool firstIdxRead = false;
                string firstIdxValue = "";
                string secondIdxValue = "";
                //Read the next set of values until we run into a new emote declaration
                while (i < emoteString.Length && emoteString[i] != '/')
                {
                    char c = emoteString[i];

                    if (c == ',')
                    {
                        firstIdxRead = false;

                        EmoteLocation loc = new EmoteLocation();
                        loc.StartIndex = int.Parse(firstIdxValue);
                        loc.EndIndex = int.Parse(secondIdxValue);
                        locations.Add(loc);

                        firstIdxValue = "";
                        secondIdxValue = "";
                    }
                    else
                    {
                        if (!firstIdxRead)
                        {
                            if (c == '-')
                            {
                                firstIdxRead = true;
                            }
                            else
                            {
                                firstIdxValue += c;
                            }
                        }
                        else
                        {
                            secondIdxValue += c;
                        }
                    }

                    i++;
                }

                emoteInfo.LocationsInMessage = locations;
                emoteInfos.Add(emoteInfo);
            }

            EmoteInfo = emoteInfos;
        }
    }
}
