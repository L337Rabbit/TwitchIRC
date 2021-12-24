using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using okitoki.twitch.irc.client;
using okitoki.twitch.irc.messaging;
using okitoki.twitch.irc.messaging.client;
using okitoki.twitch.irc.messaging.server;
using okitoki.twitch.irc.messaging.server.commands;
using okitoki.twitch.irc.messaging.server.commands.usernotice;
using okitoki.twitch.irc.messaging.server.membership;

namespace okitoki.twitch.irc
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "username";
            string oathToken = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            string credentialPassword = "";

            TwitchClient client = new TwitchClient();
            client.Credentials = new Credentials(username, oathToken);
            client.StoreCredentials();
            //client.LoadCredentials(credentialPassword);
            //client.Credentials = new Credentials(username, oathToken);
            /*client.Credentials = new Credentials(username, oathToken);
            client.StoreCredentials(credentialPassword);*/

            /*
            string channelName = "";
            client.Connect();
            client.ActivateAutoReconnect();
            client.JoinChannel(channelName, true);
            client.OnUserStateReceived += UserStateReceived;
            client.OnGlobalUserStateReceived += GlobalUserStateReceived;
            client.OnPrivateMessageReceived += PrivateMessageReceived;
            client.OnNoticeReceived += NoticeReceived;*/

            //client.ConnectToWS();

            /*
            int count = 0;
            while (true)
            {
                Thread.Sleep(5000);
                client.SendChatMessage(channel_name, "Hello " + count++ + " :)");
            }*/

            //client.LoadViewerList(channel_name);
        }

        private static void ClearAllMessagesReceived(ClearAllMessage msg)
        {
            Console.WriteLine("All chat messages cleared: " + msg.Channel + "/" + msg.User);
        }

        private static void ClearSingleMessageReceived(ClearSingleMessage msg)
        {
            Console.WriteLine("Single message cleared: " + msg.Channel + "/" + msg.User);
        }

        private static void GlobalUserStateReceived(object sender, GlobalUserStateMessage msg)
        {
            Console.WriteLine("Global user state: " + msg.DisplayName);
            Console.WriteLine("\tName Color: " + msg.NameColor);
            Console.WriteLine("\tUser Type: " + msg.UserType);
            Console.WriteLine("\tMonths Subscribed: " + msg.MonthsSubscribed);
            Console.WriteLine("\tMessage Type: " + msg.MessageType);
        }

        private static void HostTargetReceived(HostTargetMessage msg)
        {
            if (msg.TargetChannel == null)
            {
                Console.WriteLine(msg.HostingChannel + " is no longer hosting anyone.");
            }
            else
            {
                Console.WriteLine(msg.HostingChannel + " now hosting " + msg.TargetChannel + " with " + msg.ViewerCount + " viewers.");
            }
        }

        private static void JoinReceived(JoinMessage msg)
        {
            Console.WriteLine("User '" + msg.User + "' joined channel '" + msg.Channel + "'");
        }

        private static void NoticeReceived(object sender, NoticeMessage msg)
        {
            Console.WriteLine("Message " + msg.NoticeType + ": '" + msg.Message + "' deleted in channel '" + msg.Channel + "'");
        }

        private static void PartReceived(PartMessage msg)
        {
            Console.WriteLine("User '" +  msg.User+ "' left channel '" + msg.Channel + "'");
        }

        private static void PingReceived(PingMessage msg)
        {
            Console.WriteLine("Ping received.");
        }

        private static void PrivateMessageReceived(object sender, PrivateMessage msg)
        {
            Console.WriteLine("Chat message received: " + 
                "\n\tChannel: " + msg.Channel + 
                "\n\tUser Name: " + msg.Username + 
                "\n\tMessage: '" + msg.Message + "'" +
                "\n\tMessage ID: " + msg.MessageID +
                "\n\tMonths Subscribed: " + msg.MonthsSubscribed +
                "\n\tName Color: " + msg.NameColor +
                "\n\tDisplay Name: " + msg.DisplayName +
                "\n\tModerator? " + msg.Moderator +
                "\n\tRoom ID: " + msg.RoomID +
                "\n\tSent Timestamp: " + msg.SentTimestamp +
                "\n\tUser ID: " + msg.UserID +
                "\n\tTotal Bits Sent: " + msg.TotalBitsSent +
                "\n\tIs Reply? " + msg.IsReply);
        }

        private static void ReconnectReceived(ReconnectMessage msg)
        {
            Console.WriteLine("Reconnect received.");
        }

        private static void RoomStateReceived(RoomStateMessage msg)
        {
            Console.WriteLine("Room state received for channel '" + msg.Channel + "'");
            Console.WriteLine("\tSub-only: " + msg.SubOnlyMode);
            Console.WriteLine("\tEmote-Only: " + msg.EmoteOnlyMode);
            Console.WriteLine("\tFollowers-Only: " + msg.FollowersOnlyMode);
            Console.WriteLine("\tR9K: " + msg.R9KMode);
            Console.WriteLine("\tMessage-Interval: " + msg.MessageWaitTime);
        }

        private static void UserNoticeReceived(UserNoticeMessage msg)
        {
            Console.WriteLine("User notice recieved: " +
                "\n\tNotice Type: " + msg.NoticeType +
                "\n\tMonths Subscribed: " + msg.MonthsSubscribed  +
                "\n\tName Color: " + msg.NameColor +
                "\n\tDisplay Name: " + msg.DisplayName +
                "\n\tUser Name: " + msg.Username +
                "\n\tUser ID: " + msg.UserID +
                "\n\tMessage: '" + msg.Message + "'" +
                "\n\tMessage ID: " + msg.MessageId +
                "\n\tModerator: " + msg.Moderator +
                "\n\tRoom ID: " + msg.RoomID +
                "\n\tSystem Message: " + msg.SystemMessage +
                "\n\tSent Timestamp: " + msg.SentTimestamp +
                "\n\tChannel: " + msg.Channel);
        }

        private static void UserStateReceived(object sender, UserStateMessage msg)
        {
            Console.WriteLine("Global user state: " + msg.DisplayName);
            Console.WriteLine("\tName Color: " + msg.NameColor);
            Console.WriteLine("\tModerator: " + msg.Moderator);
            Console.WriteLine("\tMonths Subscribed: " + msg.MonthsSubscribed);
            Console.WriteLine("\tMessage Type: " + msg.MessageType);
        }
    }
}
