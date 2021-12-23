using okitoki.twitch.irc.messaging;
using okitoki.twitch.irc.messaging.server;
using okitoki.twitch.irc.messaging.server.commands;
using okitoki.twitch.irc.messaging.server.commands.usernotice;
using okitoki.twitch.irc.messaging.server.membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.client
{
    public abstract class MessageHandler : MessageRelay
    {
        public void ReceivedSubscriptionMessage(object sender, SubscriptionNotice message)
        {

        }

        public void ReceivedGiftedSubscriptionMessage(object sender, GiftedSubscriptionNotice message)
        {

        }

        public void ReceivedBitsBadgeUpdateMessage(object sender, BitsBadgeTierChangeNotice message)
        {

        }

        public void ReceivedGiftSubscriptionUpgradeMessage(object sender, GiftToPaidSubscriptionUpgradeNotice message)
        {

        }

        public void ReceivedPrimeSubscriptionUpgradeMessage(object sender, PrimeToPaidSubscriptionUpgradeNotice message)
        {

        }

        public void ReceivedRaidMessage(object sender, RaidNotice message)
        {

        }

        public void ReceivedUnraidMessage(object sender, UnraidNotice message)
        {

        }

        public void ReceivedRitualMessage(object sender, RitualNotice message)
        {

        }

        public void ReceivedClearAllMessage(object sender, ClearAllMessage message)
        {

        }

        public void ReceivedClearSingleMessage(object sender, ClearSingleMessage message)
        {

        }

        public void ReceivedGlobalUserStateMessage(object sender, GlobalUserStateMessage message)
        {

        }

        public void ReceivedHostMessage(object sender, HostTargetMessage message)
        {

        }

        public void ReceivedNotice(object sender, NoticeMessage message)
        {

        }

        public void ReceivedReconnect(object sender, ReconnectMessage message)
        {

        }

        public void ReceivedRoomStateMessage(object sender, RoomStateMessage message)
        {

        }

        public void ReceivedUserNoticeMessage(object sender, UserNoticeMessage message)
        {

        }

        public void ReceivedUserStateMessage(object sender, UserStateMessage message)
        {

        }

        public void ReceivedJoinMessage(object sender, JoinMessage message)
        {

        }

        public void ReceivedPartMessage(object sender, PartMessage message)
        {

        }

        public void ReceivedPrivateMessage(object sender, PrivateMessage message)
        {

        }

        public void ReceivedPingMessage(object sender, PingMessage message)
        {

        }

        public void ReceivedUnknownMessage(object sender, IRCMessage message)
        {

        }
    }
}
