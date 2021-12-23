using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public class GiftedSubscriptionNotice : UserNoticeMessage, TaggedMessage
    {
        public int TotalMonthsSubscribed { get; set; }

        public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.TIER1;

        public String SubscriptionPlanName { get; set; }

        public String RecipientDisplayName { get; set; }

        public long RecipientID { get; set; }

        public String RecipientUsername { get; set; }

        public int TotalMonthsGifted { get; set; }

        public GiftedSubscriptionNotice(string[] parts, Dictionary<string, string> tags, UserNoticeType noticeType) : base(parts, tags, noticeType)
        {
            ProcessTags();
        }

        public new void ProcessTags()
        {
            foreach (string key in tags.Keys)
            {
                switch (key)
                {
                    case "msg-param-months":
                        TotalMonthsSubscribed = int.Parse(tags["msg-param-months"]);
                        break;
                    case "msg-param-recipient-display-name":
                        RecipientDisplayName = tags["msg-param-recipient-display-name"];
                        break;
                    case "msg-param-recipient-id":
                        RecipientID = long.Parse(tags["msg-param-recipient-id"]);
                        break;
                    case "msg-param-recipient-user-name":
                        RecipientUsername = tags["msg-param-recipient-user-name"];
                        break;
                    case "msg-param-sub-plan":
                        ProcessSubPlan();
                        break;
                    case "msg-param-sub-plan-name":
                        SubscriptionPlanName = tags["msg-param-sub-plan-name"];
                        break;
                    case "msg-param-gift-months":
                        TotalMonthsGifted = int.Parse(tags["msg-param-gift-months"]);
                        break;
                }
            }
        }

        protected void ProcessSubPlan()
        {
            string subPlan = tags["msg-param-sub-plan"];

            switch (subPlan)
            {
                case "Prime": SubscriptionType = SubscriptionType.PRIME; break;
                case "1000": SubscriptionType = SubscriptionType.TIER1; break;
                case "2000": SubscriptionType = SubscriptionType.TIER2; break;
                case "3000": SubscriptionType = SubscriptionType.TIER3; break;
                default: break;
            }
        }

        public bool IsAnonymous()
        {
            return DisplayName == null || DisplayName.Equals("");
        }
    }
}
