using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging.server.commands.usernotice
{
    public enum UserNoticeType
    {
        UNKNOWN,
        GENERAL,
        SUB, 
        RESUB, 
        SUB_GIFT, 
        ANON_SUB_GIFT, 
        SUB_MYSTERY_GIFT, 
        GIFT_PAID_UPDGRADE, 
        REWARD_GIFT, 
        ANON_GIFT_PAID_UPGRADE, 
        RAID, 
        UNRAID, 
        RITUAL, 
        BITS_BADGE_TIER,
        PRIME_PAID_UPGRADE
    }
}
