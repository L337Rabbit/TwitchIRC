using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging.server.commands;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public class SubscriptionNotice : UserNoticeMessage, TaggedMessage
    {
        public int TotalMonthsSubscribed { get; set; }

        public Boolean ShareStreak { get; set; } = false;

        public int ConsecutiveMonthsSubscribed { get; set; } = 0;

        public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.TIER1;

        public String SubscriptionPlanName { get; set; }

        public SubscriptionNotice(String[] parts, Dictionary<string, string> tags, UserNoticeType noticeType) : base(parts, tags, noticeType)
        {
            ProcessTags();
        }

        public new void ProcessTags()
        {
            foreach(string key in tags.Keys)
            {
                switch(key)
                {
                    case "msg-param-cumulative-months":
                        TotalMonthsSubscribed = int.Parse(tags["msg-param-cumulative-months"]); ;
                        break;
                    case "msg-param-should-share-streak":
                        ShareStreak = !tags["msg-param-should-share-streak"].Equals("0"); ;
                        break;
                    case "msg-param-streak-months":
                        ConsecutiveMonthsSubscribed = int.Parse(tags["msg-param-streak-months"]); ;
                        break;
                    case "msg-param-sub-plan":
                        ProcessSubPlan();
                        break;
                    case "msg-param-sub-plan-name":
                        SubscriptionPlanName = tags["msg-param-sub-plan-name"]; ;
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
    }
}
