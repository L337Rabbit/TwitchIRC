using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using okitoki.twitch.irc.messaging;
using okitoki.twitch.irc.messaging.client;
using okitoki.twitch.irc.messaging.server;
using okitoki.twitch.irc.messaging.server.commands;
using okitoki.twitch.irc.messaging.server.membership;
using System.Text.Json.Serialization;
using System.Security.Cryptography;
using okitoki.twitch.irc.messaging.server.commands.usernotice;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.client
{
    public class TwitchClient : MessageHandler
    {
        public Dictionary<string, Viewer> Viewers { get; set; } = new Dictionary<string, Viewer>();

        public Dictionary<string, Channel> Channels { get; set; } = new Dictionary<string, Channel>();

        private StreamReader reader;

        private StreamWriter writer;

        private TcpClient client;

        //private Socket client;

        private Thread messageReceiverThread;

        public Credentials Credentials { get; set; }

        public bool IsConnected { get; set; } = false;

        private ClientWebSocket ws;

        public TwitchClient()
        {
            SetupRelay();
        }

        public void Connect()
        {
            if (IsConnected)
            {
                return;
            }

            try
            {
                IsConnected = true;
                Console.WriteLine("Creating client...");
                client = new TcpClient("irc.chat.twitch.tv", 6667);

                /*IPHostEntry hostEntry = Dns.GetHostEntry("irc.chat.twitch.tv");
                IPAddress ipAddress = hostEntry.AddressList[1];


                //foreach (IPAddress ip in hostEntry.AddressList) {
                    IPEndPoint endpoint = new IPEndPoint(ipAddress, 6667);
                    client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    client.Connect(endpoint);*8/

                /*    if(client.Connected)
                    {
                        break;
                    }
                }*/


                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream());
                writer.AutoFlush = true;

                Thread.Sleep(100);

                Console.WriteLine("Starting receiver thread...");
                Receive();

                Console.WriteLine("Sending credentials...");
                SendCredentials();

                Thread.Sleep(100);

                Console.WriteLine("Requesting capabilities...");
                RequestCapabilities();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }

            if (messageReceiverThread.IsAlive)
            {
                messageReceiverThread.Interrupt();
                messageReceiverThread.Abort();
            }

            if (reader != null)
            {
                reader.Close();

                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
            }

            if (writer != null)
            {
                writer.Close();

                if (writer != null)
                {
                    writer.Dispose();
                    writer = null;
                }
            }

            if (client != null)
            {
                client.Close();

                if (client != null)
                {
                    client.Dispose();
                    client = null;
                }
            }

            IsConnected = false;
        }

        private void Reconnect(object sender, ReconnectMessage message)
        {
            Reconnect();
        }

        private void Reconnect()
        {
            if (IsConnected)
            {
                Disconnect();
            }

            Thread.Sleep(5000);

            Connect();
        }

        public void ActivateAutoReconnect()
        {
            Thread reconnectThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(4900);

                    if (!IsConnected)
                    {
                        Thread.Sleep(100);

                        Console.WriteLine("Reconnecting...");
                        Reconnect();
                    }
                }
            });

            reconnectThread.Name = "Twitch-Auto-Reconnect-Thread";
            reconnectThread.Start();
        }

        private void SendCredentials()
        {
            string message = "PASS oauth:" + Credentials.OauthToken;
            SendMessage(message);

            message = "NICK " + Credentials.Username;
            SendMessage(message);
        }

        private void RequestCapabilities()
        {
            string message = "CAP REQ :twitch.tv/membership";
            SendMessage(message);

            message = "CAP REQ :twitch.tv/tags";
            SendMessage(message);

            message = "Cap REQ :twitch.tv/commands";
            SendMessage(message);
        }

        public void SendChatMessage(string channel, string message)
        {
            SendMessage("PRIVMSG #" + channel + " :" + message);
        }

        public void JoinChannel(string channelName, bool queueMessages = false)
        {
            if (!Channels.ContainsKey(channelName))
            {
                Channel channel = new Channel();
                channel.Name = channelName;
                Channels.Add(channelName, channel);

                if (queueMessages)
                {
                    channel.StartQueue();
                }
            }

            string message = "JOIN #" + channelName;
            SendMessage(message);
        }

        public void LeaveChannel(string channelName)
        {
            if (Channels.ContainsKey(channelName))
            {
                Channels[channelName].StopQueue();
                Channels.Remove(channelName);
            }

            string message = "PART #" + channelName;
            SendMessage(message);
        }

        private void Receive()
        {
            messageReceiverThread = new Thread(() => {
                string currentLine = "";
                int character;
                char c;
                byte[] buffer = new byte[1024];
                while (true)
                {
                    try
                    {
                        /*int bytesRead = 0;
                        while((bytesRead = client.Receive(buffer)) > 0)
                        {
                            for (int i = 0; i < bytesRead; i++)
                            {
                                c = (char)buffer[i];
                                currentLine += c;

                                if (c == '\n')
                                {
                                    IRCMessage message = IRCMessage.ParseMessage(currentLine);
                                    RelayMessage(message);

                                    if (message == null || message.MessageType == MessageType.UNKNOWN || (message is GeneralUserNotice))
                                    {
                                        Console.WriteLine("General/Unknown message: " + currentLine);
                                    }

                                    currentLine = null;
                                }
                            }
                        }

                        /*int numBytes;
                        if((numBytes = Math.Min(client.Available, buffer.Length)) > 0)
                        {
                            int bytesRead = client.Receive(buffer, numBytes, SocketFlags.None);

                            for (int i = 0; i < bytesRead; i++)
                            {
                                c = (char)buffer[i];
                                currentLine += c;

                                if (c == '\n')
                                {
                                    IRCMessage message = IRCMessage.ParseMessage(currentLine);
                                    RelayMessage(message);

                                    if (message == null || message.MessageType == MessageType.UNKNOWN || (message is GeneralUserNotice))
                                    {
                                        Console.WriteLine("General/Unknown message: " + currentLine);
                                    }

                                    currentLine = null;
                                }
                            }
                        }*/

                        //currentLine = reader.ReadLine();
                        /*if((character = reader.Peek()) > -1)
                        {
                            c = (char)reader.Read();
                            currentLine += c;

                            if(c == '\n')
                            {
                                IRCMessage message = IRCMessage.ParseMessage(currentLine);
                                RelayMessage(message);

                                if (message == null || message.MessageType == MessageType.UNKNOWN || (message is GeneralUserNotice))
                                {
                                    Console.WriteLine("General/Unknown message: " + currentLine);
                                }

                                currentLine = null;
                            }
                        }*/

                        currentLine = null;

                        if (!reader.EndOfStream)
                        {
                            currentLine = reader.ReadLine();
                        }

                        if (currentLine != null && currentLine.Length > 0)
                        {
                            IRCMessage message = IRCMessage.ParseMessage(currentLine);
                            RelayMessage(message);

                            if (message == null || message.MessageType == MessageType.UNKNOWN || (message is GeneralUserNotice))
                            {
                                Console.WriteLine("General/Unknown message: " + currentLine);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (currentLine != null)
                        {
                            Console.WriteLine(currentLine);
                        }
                        Console.WriteLine("An error occurred when reading from Twitch: " + e.ToString());
                        new Thread(() => Disconnect()).Start();
                        break;
                    }
                }
            });

            messageReceiverThread.Name = "Twitch_MessageReceiverThread";
            messageReceiverThread.Start();
        }

        private void SendMessage(string message)
        {
            if(!IsConnected)
            {
                return;
            }

            byte[] messageData = System.Text.Encoding.ASCII.GetBytes(message);
            Console.WriteLine("Sent data: " + message);
            writer.WriteLine(message);
        }

        public ViewerList LoadViewerList(string channelName)
        {
            HttpWebRequest viewerRequest = (HttpWebRequest)WebRequest.Create("https://tmi.twitch.tv/group/user/" + channelName + "/chatters");
            viewerRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            WebResponse response = viewerRequest.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            string viewerList = "";
            while (!reader.EndOfStream)
            {
                char c = (char)reader.Read();
                viewerList += c;
            }
            reader.Close();

            ViewerList viewers = JsonSerializer.Deserialize<ViewerList>(viewerList);
            UpdateViewersFromList(channelName, viewers);

            Console.WriteLine("Viewer count: " + viewers.ViewerCount);
            return viewers;
        }

        private void UpdateViewersFromList(string channelName, ViewerList viewers)
        {
            if (Channels.ContainsKey(channelName))
            {
                Channel channel = Channels[channelName];
                foreach (string chatter in viewers.ViewerGroups.Admins)
                {
                    channel.AddOrUpdateViewer(chatter, ViewerType.ADMIN);
                }
                foreach (string chatter in viewers.ViewerGroups.GlobalModeratorss)
                {
                    channel.AddOrUpdateViewer(chatter, ViewerType.GLOBAL_MODERATOR);
                }
                foreach (string chatter in viewers.ViewerGroups.Moderators)
                {
                    Console.WriteLine("Found mod: " + chatter);
                    channel.AddOrUpdateViewer(chatter, ViewerType.MODERATOR);
                }
                foreach (string chatter in viewers.ViewerGroups.Staff)
                {
                    channel.AddOrUpdateViewer(chatter, ViewerType.STAFF);
                }
                foreach (string chatter in viewers.ViewerGroups.Viewers)
                {
                    channel.AddOrUpdateViewer(chatter, ViewerType.VIEWER);
                }
                foreach (string chatter in viewers.ViewerGroups.Vips)
                {
                    channel.AddOrUpdateViewer(chatter, ViewerType.VIP);
                }
            }
        }

        public void SetupRelay()
        {
            //Setup default relay methods.
            OnSubReceived += ReceivedSubscriptionMessage;
            OnGiftedSubReceived += ReceivedGiftedSubscriptionMessage;
            OnBitsBadgeTierUpdateReceived += ReceivedBitsBadgeUpdateMessage;
            OnGiftSubUpgradeReceived += ReceivedGiftSubscriptionUpgradeMessage;
            OnPrimeSubUpgradeReceived += ReceivedPrimeSubscriptionUpgradeMessage;
            OnRaidMessageReceived += ReceivedRaidMessage;
            OnRitualMessageReceived += ReceivedRitualMessage;
            OnUnraidMessageReceived += ReceivedUnraidMessage;
            OnClearAllMessagesOfUserReceived += ReceivedClearAllMessage;
            OnClearSingleUserMessageReceived += ReceivedClearSingleMessage;
            OnGlobalUserStateReceived += ReceivedGlobalUserStateMessage;
            OnHostMessageReceived += ReceivedHostMessage;
            OnNoticeReceived += ReceivedNotice;
            OnReconnectMessageReceived += ReceivedReconnect;
            OnRoomStateReceived += ReceivedRoomStateMessage;
            OnUserNoticeReceived += ReceivedUserNoticeMessage;
            OnUserStateReceived += ReceivedUserStateMessage;
            OnJoinMessageReceived += ReceivedJoinMessage;
            OnPartMessageReceived += ReceivedPartMessage;
            OnPrivateMessageReceived += ReceivedPrivateMessage;
            OnPingReceived += ReceivedPingMessage;
            OnUnknownMessageReceived += ReceivedUnknownMessage;

            //Setup relay methods used by this client.
            OnReconnectMessageReceived += Reconnect;
            OnUserMessageReceived += UserMessageReceived;
            OnJoinMessageReceived += UserJoined;
            OnPartMessageReceived += UserLeft;
            OnUserNoticeReceived += UserNoticeReceived;
            OnRoomStateReceived += RoomStateReceived;
            OnChannelSpecificMessageReceived += QueueMessage;
            OnPingReceived += PingReceived;
        }

        private void PingReceived(object sender, PingMessage message)
        {
            SendMessage("PONG :tmi.twitch.tv");
        }

        private void QueueMessage(object sender, ChannelSpecificMessage message)
        {
            if (message is ChannelSpecificMessage)
            {
                string channelName = ((ChannelSpecificMessage)message).Channel;

                if (channelName != null && !channelName.Equals("") && Channels.ContainsKey(channelName))
                {
                    Channels[channelName].QueueMessage((IRCMessage)message);
                }
            }
        }

        private void RoomStateReceived(object sender, RoomStateMessage message)
        {
            if (message.Channel != null)
            {
                if (Channels.ContainsKey(message.Channel))
                {
                    Channel channel = Channels[message.Channel];

                    if (message.RoomID > -1)
                    {
                        channel.RoomID = message.RoomID;
                    }

                    channel.EmoteOnlyMode = message.EmoteOnlyMode;
                    channel.FollowersOnlyMode = message.FollowersOnlyMode;
                    channel.R9KMode = message.R9KMode;
                    channel.MessageWaitTime = message.MessageWaitTime;
                    channel.SubOnlyMode = message.SubOnlyMode;
                }
            }
        }

        private void UserNoticeReceived(object sender, UserNoticeMessage message)
        {
            if (message.Channel != null && message.RoomID > -1)
            {
                Channels[message.Channel].RoomID = message.RoomID;
            }
        }

        private void UserJoined(object sender, JoinMessage message)
        {
            Channels[message.Channel].AddOrUpdateViewer(message.User);
        }

        private void UserLeft(object sender, PartMessage message)
        {
            Channels[message.Channel].RemoveViewer(message.User);
        }

        private void UserMessageReceived(object sender, UserMessage message)
        {
            Viewer viewer = null;

            if (message.Username != null)
            {
                if (Viewers.ContainsKey(message.Username))
                {
                    viewer = Viewers[message.Username];
                }
                else
                {
                    viewer = new Viewer();
                    viewer.Username = message.Username;
                    Viewers.Add(viewer.Username, viewer);
                }
            }
            else if (message.DisplayName != null)
            {
                if (Viewers.ContainsKey(message.Username))
                {
                    viewer = Viewers[message.Username];
                }
                else
                {
                    viewer = new Viewer();
                    viewer.DisplayName = message.DisplayName;
                    Viewers.Add(viewer.DisplayName.ToLower(), viewer);
                }
            }

            if (viewer != null)
            {
                UpdateViewerValues(message, viewer);
                UpdateViewerInChannel(message, viewer);
            }
        }

        private void UpdateViewerInChannel(UserMessage message, Viewer viewer)
        {
            if (message.Channel != null)
            {
                if (Channels.ContainsKey(message.Channel))
                {
                    Channel channel = Channels[message.Channel];
                    channel.AddOrUpdateViewer(viewer.Username, message.MonthsSubscribed, message.Moderator, message.Badges);
                }
            }
        }

        private void UpdateViewerValues(UserMessage message, Viewer viewer)
        {
            if (message.DisplayName != null)
            {
                viewer.DisplayName = message.DisplayName;
                if (viewer.Username == null)
                {
                    viewer.Username = viewer.DisplayName.ToLower();
                }
            }

            if (message.Username != null)
            {
                viewer.Username = message.Username;
                if (viewer.DisplayName == null)
                {
                    viewer.DisplayName = viewer.Username;
                }
            }

            if (message.NameColor != null)
            {
                viewer.NameColor = message.NameColor;
            }

            if (message is PrivateMessage)
            {
                if (((PrivateMessage)message).UserID > -1)
                {
                    viewer.UserID = ((PrivateMessage)message).UserID;
                }
            }

            if (message is UserStateMessage)
            {
                if (((UserStateMessage)message).EmoteSets != null)
                {
                    viewer.EmoteSets = ((UserStateMessage)message).EmoteSets;
                }

                if (message is GlobalUserStateMessage)
                {
                    if (((GlobalUserStateMessage)message).UserID > -1)
                    {
                        viewer.UserID = ((GlobalUserStateMessage)message).UserID;
                    }
                }
            }

            if (message is UserNoticeMessage)
            {
                if (((UserNoticeMessage)message).UserID > -1)
                {
                    viewer.UserID = ((UserNoticeMessage)message).UserID;
                }
            }
        }

        public void StoreCredentials(string password, string filePath = "tc.crd")
        {
            if (Credentials != null)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (StreamWriter writer = new StreamWriter(filePath))
                {

                    string json = JsonSerializer.Serialize(Credentials, typeof(Credentials));
                    writer.WriteLine(Credentials.Encrypt(password, json));
                    writer.Flush();
                }
            }
        }

        public Credentials LoadCredentials(string password, string filePath = "tc.crd")
        {
            Credentials credentials = null;

            if (File.Exists(filePath))
            {
                string creds = null;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        creds += reader.ReadLine();
                    }
                }

                if (creds != null)
                {
                    string json = Credentials.Decrypt(password, creds);
                    Console.WriteLine("Credentials JSON: " + json);
                    credentials = JsonSerializer.Deserialize<Credentials>(json);
                    this.Credentials = credentials;
                }
            }

            return credentials;
        }
    }
}
