using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public class NoticeMessage : IRCMessage, ChannelSpecificMessage
    {
        public string Channel { get; set; }

        public string Message { get; set; }

        public NoticeType NoticeType { get; set; }

        public NoticeMessage(string[] parts) : base(MessageType.NOTICE)
        {
            int indexOffset = 0;

            //Parse the message
            if (parts[0].StartsWith("@"))
            {
                tags = ParseTags(parts[0]);

                if(tags.ContainsKey("msg-id"))
                {
                    string noticeType = tags["msg-id"].ToUpper();
                    ProcessNoticeType(noticeType);
                }

                indexOffset++;
            }

            if (parts[indexOffset + 2][0] == '#')
            {
                Channel = parts[indexOffset + 2].Substring(1);
                indexOffset++;
            }

            Message = ExtractMessage(parts, indexOffset + 2);
        }

        public override string ToString()
        {
            return ":tmi.twitch.tv NOTICE #" + Channel + " :" + Message;
        }

        private void ProcessNoticeType(string noticeTypeString)
        {
            NoticeType noticeType = NoticeType.UNKNOWN;
            
            if(Enum.TryParse<NoticeType>(noticeTypeString, out noticeType))
            {
                NoticeType = noticeType;
            }
        }
    }
}
