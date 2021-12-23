using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public class GiftToPaidSubscriptionUpgradeNotice : UserNoticeMessage
    {
        public int TotalSubsGiftedDuringPromo { get; set; }

        public String PromoName { get; set; }

        public String GifterUsername { get; set; }

        public String GifterDisplayName { get; set; }

        public GiftToPaidSubscriptionUpgradeNotice(string[] parts, Dictionary<string, string> tags, UserNoticeType noticeType) : base(parts, tags, noticeType)
        {
            ProcessTags();
        }

        public new void ProcessTags()
        {
            foreach (string key in tags.Keys)
            {
                switch (key)
                {
                    case "msg-param-promo-gift-total":
                        TotalSubsGiftedDuringPromo = int.Parse(tags["msg-param-promo-gift-total"]);
                        break;
                    case "msg-param-promo-name":
                        PromoName = tags["msg-param-promo-name"];
                        break;
                    case "msg-param-sender-login":
                        GifterUsername = tags["msg-param-sender-login"];
                        break;
                    case "msg-param-sender-name":
                        GifterDisplayName = tags["msg-param-sender-name"];
                        break;
                }
            }
        }

        public bool IsAnonymous()
        {
            return (GifterUsername == null || GifterUsername.Equals("")) && (GifterDisplayName == null || GifterDisplayName.Equals(""));
        }
    }
}
