using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;
using okitoki.twitch.irc.messaging.client;
using okitoki.twitch.irc.messaging.server;
using okitoki.twitch.irc.messaging.server.commands;
using okitoki.twitch.irc.messaging.server.membership;

namespace okitoki.twitch.irc.messaging
{
    public abstract class IRCMessage
    {
        protected Dictionary<string, string> tags = new Dictionary<string, string>();

        public MessageType MessageType { get; set; } = MessageType.UNKNOWN;

        public long CreationTime { get; }

        public IRCMessage(MessageType messageType)
        {
            this.MessageType = messageType;
            CreationTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        protected bool HasTags(string message)
        {
            if(message[0] == '@')
            {
                return true;
            }

            return false;
        }

        protected string ExtractMessage(string[] messageParts, int startIndex)
        {
            string content = "";
            bool firstPart = true;
            for (int i = startIndex; i < messageParts.Length; i++)
            {
                string part = messageParts[i];

                if (firstPart)
                {
                    part = part.Substring(1);
                    firstPart = false;
                }

                content += part;
                if (i < messageParts.Length - 1)
                {
                    content += " ";
                }
            }

            return content;
        }

        protected static Dictionary<string, string> ParseTags(string tagString)
        {
            Dictionary<string, string> tags = new Dictionary<string, string>();

            if (tagString[0] != '@')
            {
                return tags;
            }

            try
            {
                string key, value;

                for (int i = 0; i < tagString.Length; i++)
                {
                    key = "";
                    value = "";
                    char c = tagString[i];

                    if (c == ';' || c == '@')
                    {
                        continue;
                    }

                    while (c != ' ' && c != '=')
                    {
                        key += c;
                        i++;

                        if (i > tagString.Length - 1) { break; }

                        c = tagString[i];
                    }

                    if (i > tagString.Length - 1) { break; }

                    if (c == '=')
                    {
                        i++;

                        if (i > tagString.Length - 1) { break; }

                        c = tagString[i];
                    }

                    if (i > tagString.Length - 1) { break; }

                    while (c != ' ' && c != ';')
                    {
                        value += c;
                        i++;

                        if (i > tagString.Length - 1) { break; }

                        c = tagString[i];
                    }

                    if (i > tagString.Length - 1) { break; }

                    if (key.Length > 0 && value.Length > 0)
                    {
                        tags.Add(key, value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred when parsing tags: " + e.ToString());
            }

            return tags;
        }

        protected string GetTagString()
        {
            string tagString = "@";
            bool first = true;

            foreach(string key in tags.Keys)
            {
                if(first)
                {
                    first = false;
                }
                else 
                { 
                    tagString += ";";
                }

                tagString += key + "=" + tags[key];
            }

            return tagString;
        }

        public static IRCMessage ParseMessage(string message)
        {
            //Split into parts and determine message type.
            string[] parts = message.Split(' ');
            int idxOffset = 0;

            if(parts[0][0] == '@')
            {
                idxOffset++;
            }

            switch(parts[idxOffset])
            {
                case "PING": return new PingMessage(message);
                case "RECONNECT": return new ReconnectMessage(parts);
            }

            switch(parts[idxOffset + 1])
            {
                case "JOIN": return new JoinMessage(parts);
                case "PART": return new PartMessage(parts);
                case "CLEARCHAT": return new ClearAllMessage(parts);
                case "CLEARMSG": return new ClearSingleMessage(parts);
                case "GLOBALUSERSTATE": return new GlobalUserStateMessage(parts);
                case "HOSTTARGET": return new HostTargetMessage(parts);
                case "NOTICE": return new NoticeMessage(parts);
                case "PRIVMSG": return new PrivateMessage(parts);
                case "RECONNECT": return new ReconnectMessage(parts);
                case "ROOMSTATE": return new RoomStateMessage(parts);
                case "USERNOTICE": return UserNoticeMessage.ParseMessage(parts);
                case "USERSTATE": return new UserStateMessage(parts);
            }

            return null;
        }
    }
}
