using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public class UserStateMessage : UserMessage
    {

        public string[] EmoteSets { get; set; }

        public UserStateMessage(string[] parts) : this(parts, MessageType.USER_STATE) { }

        protected UserStateMessage(string[] parts, MessageType messageType) : base(parts, messageType)
        {
            ProcessTags();
        }

        public override string ToString()
        {
            return ":tmi.twitch.tv USERSTATE #" + Channel;
        }

        protected void ProcessEmoteSets()
        {
            string emoteSets = tags["emote-sets"];
            string[] sets = emoteSets.Split(',');
            EmoteSets = new string[sets.Length];
            for (int i = 0; i < sets.Length; i++)
            {
                EmoteSets[i] =sets[i];
            }
        }

        public new void ProcessTags()
        {
            foreach (string key in tags.Keys)
            {
                switch (key)
                {
                    case "emote-sets":
                        ProcessEmoteSets();
                        break;
                }
            }
        }
    }
}
