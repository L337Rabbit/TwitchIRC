using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public class PrimeToPaidSubscriptionUpgradeNotice : UserNoticeMessage, TaggedMessage
    {
        public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.TIER1;

        public PrimeToPaidSubscriptionUpgradeNotice(string[] parts, Dictionary<string, string> tags) : base(parts, tags, UserNoticeType.PRIME_PAID_UPGRADE)
        {
            if(tags.Count > 0 && tags.ContainsKey("msg-param-sub-plan"))
            {
                ProcessSubPlan();
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
    }
}
