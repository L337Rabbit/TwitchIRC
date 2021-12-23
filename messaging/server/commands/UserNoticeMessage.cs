using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging;
using okitoki.twitch.irc.messaging.server.commands.usernotice;

namespace okitoki.twitch.irc.messaging.server.commands
{
    public class UserNoticeMessage : UserMessage, TaggedMessage
    {
        public UserNoticeType NoticeType { get; set; }

        public long UserID { get; set; }

        public string Message { get; set; }

        public string MessageId { get; set; }

        public long RoomID { get; set; }

        public string SystemMessage { get; set; }

        public long SentTimestamp { get; set; }

        protected UserNoticeMessage(string[] parts, Dictionary<string, string> tags, UserNoticeType noticeType) : base(parts, tags, MessageType.USER_NOTICE)
        {
            this.NoticeType = noticeType;
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

        public new void ProcessTags()
        {
            foreach (string key in tags.Keys)
            {
                switch (key)
                {
                    case "id":
                        MessageId = tags["id"];
                        break;
                    case "login":
                        Username = tags["login"];
                        break;
                    case "message":
                        Message = tags["message"];
                        break;
                    case "room-id":
                        this.RoomID = long.Parse(tags["room-id"]);
                        break;
                    case "system-msg":
                        this.SystemMessage = tags["system-msg"].Replace("\\s", " ");
                        break;
                    case "tmi-sent-ts":
                        this.SentTimestamp = long.Parse(tags["tmi-sent-ts"]);
                        break;
                    case "user-id":
                        this.UserID = long.Parse(tags["user-id"]);
                        break;
                }
            }
        }

        protected static UserNoticeMessage CreateNotice(string[] parts, Dictionary<string, string> tags)
        {
            if (tags.ContainsKey("msg-id"))
            {
                string value = tags["msg-id"];
                switch(value)
                {
                    case "sub": return new SubscriptionNotice(parts, tags, UserNoticeType.SUB);
                    case "resub": return new SubscriptionNotice(parts, tags, UserNoticeType.RESUB);
                    case "subgift": return new GiftedSubscriptionNotice(parts, tags, UserNoticeType.SUB_GIFT) ;
                    case "anonsubgift": return new GiftedSubscriptionNotice(parts, tags, UserNoticeType.ANON_SUB_GIFT) ;
                    case "giftpaidupgrade": return new GiftToPaidSubscriptionUpgradeNotice(parts, tags, UserNoticeType.GIFT_PAID_UPDGRADE) ;
                    case "anongiftpaidupgrade": return new GiftToPaidSubscriptionUpgradeNotice(parts, tags, UserNoticeType.ANON_GIFT_PAID_UPGRADE) ;
                    case "raid": return new RaidNotice(parts, tags) ;
                    case "unraid": return new UnraidNotice(parts, tags);
                    case "ritual": return new RitualNotice(parts, tags);
                    case "bitsbadgetier": return new BitsBadgeTierChangeNotice(parts, tags);
                    case "primepaidupgrade": return new PrimeToPaidSubscriptionUpgradeNotice(parts, tags);
                }
            }

            return new GeneralUserNotice(parts, tags);
        }

        public static UserNoticeMessage ParseMessage(string[] parts)
        {
            if (parts[0].StartsWith("@"))
            {
                Dictionary<string, string> tags = ParseTags(parts[0]);
                if(tags.ContainsKey("msg-id"))
                {
                    return CreateNotice(parts, tags);
                }
            }

            return new GeneralUserNotice(parts, null);
        }
     }
}
