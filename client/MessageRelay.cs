using okitoki.twitch.irc.messaging;
using okitoki.twitch.irc.messaging.server.membership;
using okitoki.twitch.irc.messaging.server.commands;
using okitoki.twitch.irc.messaging.server.commands.usernotice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using okitoki.twitch.irc.messaging.client;
using okitoki.twitch.irc.messaging.server;

namespace okitoki.twitch.irc.client
{
    public class MessageRelay
    {
        public EventHandler<GeneralUserNotice> OnGeneralUserNoticeReceived; //
        public EventHandler<SubscriptionNotice> OnSubReceived; //
        public EventHandler<GiftedSubscriptionNotice> OnGiftedSubReceived; //
        public EventHandler<BitsBadgeTierChangeNotice> OnBitsBadgeTierUpdateReceived; //
        public EventHandler<GiftToPaidSubscriptionUpgradeNotice> OnGiftSubUpgradeReceived; //
        public EventHandler<PrimeToPaidSubscriptionUpgradeNotice> OnPrimeSubUpgradeReceived; //
        public EventHandler<RaidNotice> OnRaidMessageReceived; //
        public EventHandler<RitualNotice> OnRitualMessageReceived; //
        public EventHandler<UnraidNotice> OnUnraidMessageReceived; //
        public EventHandler<ClearAllMessage> OnClearAllMessagesOfUserReceived; //
        public EventHandler<ClearSingleMessage> OnClearSingleUserMessageReceived; //
        public EventHandler<GlobalUserStateMessage> OnGlobalUserStateReceived; //
        public EventHandler<HostTargetMessage> OnHostMessageReceived; //
        public EventHandler<NoticeMessage> OnNoticeReceived; //
        public EventHandler<ReconnectMessage> OnReconnectMessageReceived; //
        public EventHandler<RoomStateMessage> OnRoomStateReceived; //
        public EventHandler<UserNoticeMessage> OnUserNoticeReceived; //
        public EventHandler<UserStateMessage> OnUserStateReceived; //
        public EventHandler<JoinMessage> OnJoinMessageReceived; //
        public EventHandler<PartMessage> OnPartMessageReceived; //
        public EventHandler<PrivateMessage> OnPrivateMessageReceived; //
        public EventHandler<PingMessage> OnPingReceived; //
        public EventHandler<IRCMessage> OnAnyMessageReceived; //
        public EventHandler<ChannelSpecificMessage> OnChannelSpecificMessageReceived; //

        protected internal EventHandler<IRCMessage> OnUnknownMessageReceived;
        protected internal EventHandler<UserMessage> OnUserMessageReceived;

        protected void RelayMessage(IRCMessage message)
        {
            if(message == null)
            {
                return;
            }

            if (OnAnyMessageReceived != null)
            {
                OnAnyMessageReceived(this, message);
            }

            switch(message.MessageType)
            {
                case MessageType.CLEAR_ALL:
                    if (OnClearAllMessagesOfUserReceived != null)
                    {
                        OnClearAllMessagesOfUserReceived(this, (ClearAllMessage)message);
                    }
                    break;
                case MessageType.CLEAR_SINGLE:
                    if (OnClearSingleUserMessageReceived != null)
                    {
                        OnClearSingleUserMessageReceived(this, (ClearSingleMessage)message);
                    }
                    break;
                case MessageType.GLOBAL_USER_STATE:
                    if (OnGlobalUserStateReceived != null)
                    {
                        OnGlobalUserStateReceived(this, (GlobalUserStateMessage)message);
                    }
                    break;
                case MessageType.HOST_TARGET:
                    if (OnHostMessageReceived != null)
                    {
                        OnHostMessageReceived(this, (HostTargetMessage)message);
                    }
                    break;
                case MessageType.JOIN:
                    if (OnJoinMessageReceived != null)
                    {
                        OnJoinMessageReceived(this, (JoinMessage)message);
                    }
                    break;
                case MessageType.NOTICE:
                    if (OnNoticeReceived != null)
                    {
                        OnNoticeReceived(this, (NoticeMessage)message);
                    }
                    break;
                case MessageType.PART:
                    if (OnPartMessageReceived != null)
                    {
                        OnPartMessageReceived(this, (PartMessage)message);
                    }
                    break;
                case MessageType.PING:
                    if (OnPingReceived != null)
                    {
                        OnPingReceived(this, (PingMessage)message);
                    }
                    break;
                case MessageType.PRIVATE_MESSAGE:
                    if (OnPrivateMessageReceived != null)
                    {
                        OnPrivateMessageReceived(this, (PrivateMessage)message);
                    }
                    break;
                case MessageType.RECONNECT:
                    if (OnReconnectMessageReceived != null)
                    {
                        OnReconnectMessageReceived(this, (ReconnectMessage)message);
                    }
                    break;
                case MessageType.ROOM_STATE:
                    if (OnRoomStateReceived != null)
                    {
                        OnRoomStateReceived(this, (RoomStateMessage)message);
                    }
                    break;
                case MessageType.USER_NOTICE:
                    if (OnUserNoticeReceived != null)
                    {
                        OnUserNoticeReceived(this, (UserNoticeMessage)message);
                    }
                    HandleUserNotice((UserNoticeMessage)message);
                    break;
                case MessageType.USER_STATE:
                    if (OnUserStateReceived != null)
                    {
                        OnUserStateReceived(this, (UserStateMessage)message);
                    }
                    break;
            }

            if(OnUserMessageReceived != null && message is UserMessage)
            {
                OnUserMessageReceived(this, (UserMessage)message);
            }

            if(OnChannelSpecificMessageReceived != null && message is ChannelSpecificMessage)
            {
                OnChannelSpecificMessageReceived(this, (ChannelSpecificMessage)message);
            }
        }

        private void HandleUserNotice(UserNoticeMessage message)
        {
            switch(message.NoticeType)
            {
                case UserNoticeType.ANON_GIFT_PAID_UPGRADE:
                    if(OnGiftSubUpgradeReceived != null)
                    {
                        OnGiftSubUpgradeReceived(this, (GiftToPaidSubscriptionUpgradeNotice)message);
                    }
                    break;
                case UserNoticeType.ANON_SUB_GIFT:
                    if(OnGiftedSubReceived != null)
                    {
                        OnGiftedSubReceived(this, (GiftedSubscriptionNotice)message);
                    }
                    break;
                case UserNoticeType.BITS_BADGE_TIER:
                    if(OnBitsBadgeTierUpdateReceived != null)
                    {
                        OnBitsBadgeTierUpdateReceived(this, (BitsBadgeTierChangeNotice)message);
                    }
                    break;
                case UserNoticeType.GENERAL:
                    if(OnGeneralUserNoticeReceived != null)
                    {
                        OnGeneralUserNoticeReceived(this, (GeneralUserNotice)message);
                    }
                    break;
                case UserNoticeType.GIFT_PAID_UPDGRADE:
                    if (OnGiftSubUpgradeReceived != null)
                    {
                        OnGiftSubUpgradeReceived(this, (GiftToPaidSubscriptionUpgradeNotice)message);
                    }
                    break;
                case UserNoticeType.PRIME_PAID_UPGRADE:
                    if(OnPrimeSubUpgradeReceived != null)
                    {
                        OnPrimeSubUpgradeReceived(this, (PrimeToPaidSubscriptionUpgradeNotice)message);
                    }
                    break;
                case UserNoticeType.RAID:
                    if(OnRaidMessageReceived != null)
                    {
                        OnRaidMessageReceived(this, (RaidNotice)message);
                    }
                    break;
                case UserNoticeType.RESUB:
                    if(OnSubReceived != null)
                    {
                        OnSubReceived(this, (SubscriptionNotice)message);
                    }
                    break;
                case UserNoticeType.RITUAL:
                    if(OnRitualMessageReceived != null) 
                    {
                        OnRitualMessageReceived(this, (RitualNotice)message);
                    }
                    break;
                case UserNoticeType.SUB:
                    if(OnSubReceived != null)
                    {
                        OnSubReceived(this, (SubscriptionNotice)message);
                    }
                    break;
                case UserNoticeType.SUB_GIFT:
                    if (OnGiftedSubReceived != null)
                    {
                        OnGiftedSubReceived(this, (GiftedSubscriptionNotice)message);
                    }
                    break;
                case UserNoticeType.UNRAID:
                    if(OnUnraidMessageReceived != null)
                    {
                        OnUnraidMessageReceived(this, (UnraidNotice)message);
                    }
                    break;
            }
        }
    }
}
