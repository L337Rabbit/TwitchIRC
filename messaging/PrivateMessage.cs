using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;
using okitoki.twitch.irc.messaging.server.commands;

namespace okitoki.twitch.irc.messaging
{
    public class PrivateMessage : UserMessage, TaggedMessage, ChannelSpecificMessage
    {
        public string Message { get; set; }

        public string MessageID { get; set; }

        public long RoomID { get; set; } = -1;

        public long SentTimestamp { get; set; }

        public long UserID { get; set; } = -1;

        public int TotalBitsSent { get; set; }

        public bool IsReply { get; set; }

        public string ReplyParentMessageID { get; set; }
        
        public string ReplyParentUserID { get; set; }

        public string ReplyParentUserLogin { get; set; }

        public string ReplyParentDisplayName { get; set; }

        public string ReplyParentMessageBody { get; set; }


        public PrivateMessage(string[] parts) : base(parts, MessageType.PRIVATE_MESSAGE)
        {
            int indexOffset = 0;

            if (tags.Count > 0)
            {
                ProcessTags();
                indexOffset++;
            }

            if (this.Message == null)
            {
                Message = ExtractMessage(parts, indexOffset + 3);
            }
        }

        public override string ToString()
        {
            return "PRIVMSG #" + Channel + " :" + Message;
        }

        public bool IsBitsMessage()
        {
            return TotalBitsSent > 0;
        }

        public new void ProcessTags()
        {
            foreach(string key in tags.Keys)
            {
                switch(key)
                {
                    case "bits":
                        TotalBitsSent = int.Parse(tags["bits"]);
                        break;
                    case "room-id":
                        this.RoomID = long.Parse(tags["room-id"]); ;
                        break;
                    case "tmi-sent-ts":
                        this.SentTimestamp = long.Parse(tags["tmi-sent-ts"]);
                        break;
                    case "user-id":
                        this.UserID = long.Parse(tags["user-id"]); ;
                        break;
                    case "message":
                        Message = tags["message"]; ;
                        break;
                    case "id":
                        MessageID = tags["id"]; ;
                        break;
                    case "reply-parent-msg-id":
                        ReplyParentMessageID = tags["reply-parent-msg-id"];
                        IsReply = true;
                        break;
                    case "reply-parent-user-id":
                        ReplyParentUserID = tags["reply-parent-user-id"]; ;
                        break;
                    case "reply-parent-user-login":
                        ReplyParentUserLogin = tags["reply-parent-user-login"]; ;
                        break;
                    case "reply-parent-display-name":
                        ReplyParentDisplayName = tags["reply-parent-display-name"]; ;
                        break;
                    case "reply-parent-message-body":
                        ReplyParentMessageBody = tags["reply-parent-message-body"]; ;
                        break;
                }
            }
        }
    }
}
