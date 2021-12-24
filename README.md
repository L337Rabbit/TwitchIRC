# TwitchIRC
This is a C#/.NET API for sending and receiving Twitch IRC chat messages.

For more information on Twitch's IRC interface see https://dev.twitch.tv/docs/irc/guide.

# Installation/Setup
To use this client library, you only need the DLLs found in the **releases** folder. Currently only .NET versions 4.6.1, 4.7.1, and 4.7.2 are precompiled, but these should work with any version of .NET higher than those versions. For example: if you are using .NET 4.7.3 or higher, any of the release versions should work. If you are using .NET 4.5.0, none of the pre-compiled releases will work and you will need to pull the code, patch, and recompile for your targeted framework version.

# Unity Installation
To use in Unity, just copy all of the DLL files from the appropriate **releases** folder to anywhere in your Unity project's **Assets** folder.

# Security/Authentication
To get started, you will need to get an OAUTH token from Twitch at https://twitchapps.com/tmi/. See https://dev.twitch.tv/docs/irc/guide for further info.

Do not share your OAUTH token with anyone. If you're doing development on stream, read further down to see 2 methods for keeping your token a secret.

# Creating a Simple Chat Client
The heart of this Twitch IRC API is the TwitchClient class. To create a new TwitchClient, you will need to set credentials using a twitch username and OAUTH token (see previous section for that). You can then create a client by doing the following:

```csharp
using okitoki.twitch.irc.client;

string login = "twitch_login_username";
string oauthToken = "twitch_user_token";

TwitchClient client = new TwitchClient();
client.Credentials = new Credentials(login, oauthToken);
client.ActivateAutoReconnect();
client.Connect();
```

Note that the OAUTH token should NOT include the "oauth:" portion.

# Joining a Twitch Channel
Once you have a client, you can then use it to join any Twitch channel (as long as the user you are using isn't banned in that channel):

```csharp
string channelName = "rabbit_xxxx";
client.JoinChannel(channelName);
```

If you want the TwitchClient to queue messages up so that you can process them later, you can set the optional queueMessages paramter to true when you call JoinChannel:

```csharp
client.JoinChannel(channelName, true);
```

# Keeping Your Token a Secret (Method 1)
There are some mechanisms built into this API to help keep your OAUTH token a secret. One method you can use is to encrypt the token to a local file and load it in at runtime. The first time you create a Twitch client, you can tell it to store the credentials (username and OAUTH token). The encrypted credentials are stored in a file called "tc.crd" by default:

```csharp
TwitchClient client = new TwitchClient();
client.Credentials = new Credentials(login, oauthToken);
client.EncryptCredentials(myEncryptionPassword);
```

Once you have run the program and stored the credentials, change the code by deleting the lines where credentials are being set and stored and just call DecryptCredentials():

```csharp
TwitchClient client = new TwitchClient();
client.DecryptCredentials(myEncryptionPassword);
```

You can further increase the security of the token by using a password input box to enter the "myEncryptionPassword" password secretly prior to creating the Twitch client and loading credentials.

# Keeping Your Token a Secret (Method 2)
As an alternative to method 1 above, you can create a credentials file and manually add your login username and OAUTH token to it (unencrypted), then load this credential file when your program runs. The file contents should look like the following:

```json
{"username":"your_twitch_username","oauth-token":"your_oauth_token"}
```

Once you have the credentials file in place, you can load it using the LoadCredentials() method:

```csharp
string credentialsFilePath = "C:/Users/me/Desktop/credentials.crd";

TwitchClient client = new TwitchClient();
client.LoadCredentials(credentialsFilePath);
```

This method is less secure since anyone with access to the credentials file has access to your OAUTH token, but may be a bit easier to set up. If you use this approach, make sure not to open the credential file on stream or share it with anyone, including accidentally committing the file to a code repository like Github.

# Handling Specific Message Types
This API supports nearly the entirety of the Twitch IRC message library and is too extensive to cover every possible message type in this README; however, the more common cases are presented in the **Examples** section below. 

Twitch sends a number of different types of messages to IRC clients notifying them of chat messages, subs, gifted subs, hosts, raids, and more. Each type of message is a different *type* in this API and contains various fields/properties relevant to the type of message. Not all fields will always be set so it is up to the end-user of the API to check for null values during processing. 

All messages can be received and relayed to your own methods for processing using the event handlers of the TwitchClient. The various types of message event handlers available are described in the following table.

## Message Handler Table

| Handler Name | Message Type | Description |
| --- | --- | --- |
| OnPrivateMessageReceived | PrivateMessage | Called when a chat message from a specific user comes in. |
| OnSubReceived | SubscriptionNotice | Called when a subscription notification comes in. |
| OnGiftedSubReceived | GiftedSubscriptionNotice | Called when a gifted subscription comes in. |
| OnRaidMessageReceived | RadiNotice | Called when a raid notification comes in. |
| OnHostMessageReceived | HostTargetMessage | Called when a host notification comes in. |
| OnUserNoticeReceived | UserNoticeMessage | Called when a notice about a specific user in chat comes in. |
| OnUserStateReceived | UserStateMessage | Called when channel-specific state information about a specific user in chat comes in. |
| OnGlobalUserStateReceived | GlobalUserStateMessage | Called when global state information about a specific user in chat comes in. This |
| OnUnraidMessageReceived | UnraidNotice | Called when an unraid notification comes in. |
| OnRoomStateReceived | RoomStateMessage | Called when a room state notification comes in. |
| OnJoinMessageReceived | JoinMessage | Called when a viewer enters a joined channel. |
| OnPartMessageReceived | PartMessage | Called when a viewer leaves a joined channel. |
| OnNoticeReceived | NoticeMessage | Called when a NOTICE is received. See https://dev.twitch.tv/docs/irc/msg-id for NOTICE types. |
| OnBitsBadgeTierUpdateReceived | BitsBadgeTierChangeNotice | Called when a notification about a viewer's bits badge being upgraded comes in. |
| OnGiftSubUpgradeReceived | GiftToPaidSubscriptionUpgradeNotice | Called when a viewer converts from a gifted sub to a paid sub. |
| OnPrimeSubUpgradeReceived | PrimeToPaidSubscriptionUpgradeNotice | Called when a viewer converts from an Amazon prime sub to a paid sub. |
| OnRitualMessageReceived | RitualNotice | Called when a ritual notification is received. Currently this only indicates when a new chatter has joined. |
| OnClearAllMessagesOfUserReceived | ClearAllMessage | Called when a notification has been received indicating that all messages of a specific viewer have been deleted. |
| OnClearSingleUserMessageReceived | ClearSingleMessage | Called when a notification has been received indicating that a single message of a specific viewer was deleted. |
| OnReconnectMessageReceived | ReconnectMessage | Called when the Twitch server is indicating to the client to reconnect. |
| OnPingReceived | PingMessage | Called when a ping is received from Twitch servers. |
| OnAnyMessageReceived | IRCMessage | Called when any type of message is received by the client. |
| OnChannelSpecificMessageReceived | ChannelSpecificMessage | Called when any type of message specific to a channel is received. |
| OnGeneralUserNoticeReceived | GeneralUserNotice | Called when an unrecognized type of USER NOTICE is received. |

## General Approach
To process messages as they come in, you just need to define your own methods and add them to the appropriate handler. The general steps are outlined below:

1. Define a method in your own code with the following format (you can call the method whatever, just make sure the parameter types match):

```csharp
void SomeMessageReceived(object sender, **Message Type** msg) {...}
```

2. Add your method to the appropriate TwitchClient handler:

```csharp
client.OnSomeMessageReceived += SomeMessageReceived;
```

Now, whenever a message comes in, your method will run!

## Examples:
### Processing Viewer Messages
To process messages from individual users in chat, create a method and add it to the OnPrivateMessageReceived handler of the TwitchClient. Whenever a PrivateMessage comes in, it will be sent to your method and you can handle it appropriately.

1. Define a method for processing chat messages:

```csharp
void ChatMessageReceived(object sender, PrivateMessage msg) 
{
    //Do whatever you want here.
    //Console.WriteLine("Received private message: " + msg.Channel + "/" + msg.Username + ": " + msg.Message);
}
```

2. Add your method to the appropriate TwitchClient handler:

```csharp
client.OnPrivateMessageReceived += ChatMessageReceived;
```

### Tracking Viewers Entering/Leaving
To track when viewers enter and leave a joined channel, you should create methods and add them to the OnJoinMessageReceived and OnPartMessageReceived handlers of the TwitchClient. Note: Messages about viewers joining and leaving are not sent by Twitch servers as they occur, but rather they are queued and sent periodically.

1. Define method two methods. One for processing when viewers enter and another for processing when viewers leave:

```csharp
void ViewerJoined(object sender, JoinMessage msg) 
{
    //Do whatever you want here.
    //Console.WriteLine("" + msg.User + " joined " + msg.Channel + "'s channel.");
}

void ViewerLeft(object sender, PartMessage msg) 
{
    //Do whatever you want here.
    //Console.WriteLine("" + msg.User + " left " + msg.Channel + "'s channel.");
}
```

2. Add your methods to the appropriate TwitchClient handlers:

```csharp
client.OnJoinMessageReceived += ViewerJoined;
client.OnPartMessageReceived += ViewerLeft;
```

### Detecting Subscriptions
If you need to incorporate logic whenever normal paid subscriptions come in, you can process SubscriptionNotices by adding your own method to the OnSubReceived handler of the TwitchClient.

1. Define a method for processing subscription messages:

```csharp
void SubReceived(object sender, SubscriptionNotice msg) 
{
    //Do whatever you want here.
    //Console.WriteLine("" + msg.Username + " subscribed to " + msg.Channel + " for " + msg.TotalMonthsSubscribed + " months total.");
}
```

2. Add your method to the appropriate TwitchClient handler:

```csharp
client.OnSubReceived += SubReceived;
```

### Detecting Raids
To detect and handle raids, add a method to the OnRaidMessageReceived handler of the TwitchClient.

1. Define a method for processing raids:

```csharp
void RaidReceived(object sender, RaidNotice msg) 
{
    //Do whatever you want here.
    //Console.WriteLine("" + msg.RaiderDisplayName + " is raiding " + msg.Channel + " with " + msg.ViewerCount + " viewers.");
}
```

2. Add your method to the appropriate TwitchClient handler:

```csharp
client.OnRaidMessageReceived += RaidReceived;
```

### Detecting Hosts
To detect and handle hosts, add a method to the OnHostMessageReceived handler of the TwitchClient.

1. Define a method for processing channel hosts:

```csharp
void HostReceived(object sender, HostTargetMessage msg) 
{
    //Do whatever you want here.
    //Console.WriteLine("Received host message. " + msg.HostingChannel + " is now hosting " + msg.TargetChannel + " with " + msg.ViewerCount + " viewers.");
}
```

2. Add your method to the appropriate TwitchClient handler:

```csharp
client.OnHostMessageReceived += HostReceived;
```

# Sending Messages in Chat
To send a message in chat, just use the SendChatMessage() method after joining a channel:

```csharp
string channelName = ...;
string message = "Goodbye world :)";
client.SendChatMessage(channelName, message);
```

# Getting Information About Viewers
The TwtichClient can automatically gather certain information about viewers as they send messages in chat. The amount of information obtained about viewers using this mechanism is limited, but includes the following:

| Property Name | Description |
| --- | --- |
| UserID | The numeric Twitch ID assigned to the user. Example: 401223 |
| Username | The login name of the user. This is identical to display name except all lowercase. |
| DisplayName | The name of the user as displayed in Twitch chat with correct capitalization. |
| NameColor | The hex color code used in chat for this user's name. |
| EmoteSets | A List of IDs for the emote sets which the user has available. |

A list of Viewer data can be obtained from the TwitchClient directly. Each key in the Dictionary is the viewer's login name.

```csharp
Dictionary<string, Viewer> viewers = client.Viewers;
```

To get global information about a specific viewer, use the GetViewer() method:

```csharp
string viewerName = ...;
Viewer viewer = client.GetViewer(viewerName);

if(viewer != null) 
{
    //Do something...
}
```

Additionally, the TwitchClient can gather information about viewers on a per-channel basis. This information includes the following:

| Property Name | Description |
| --- | --- |
| MonthsSubscribed | The number of months the viewer has been subscribed to the channel. |
| IsMod | Whether the viewer is a mod in the channel. |
| ViewerType | The type of viewer (ADMIN, BROADCASTER, GLOBAL_MODERATOR, MODERATOR, STAFF, VIEWER, or VIP). |
| Badges | A collection of Badges applicable to the viewer. Usually displayed to the left of the viewer's name in Twitch chat. |

To get channel specific information about a viewer (if available), use the GetViewerInfo() method:

```csharp
string channelName = ...;
string viewerName = ...;
ViewerChannelInfo viewerInfo = client.GetViewerInfo(channelName, viewerName);

if(viewerInfo != null) 
{
    //Do something...
}
```

Note that not all information about viewers will be available at all times, since certain information only comes in with certain types of messages. You will need to check for null values on various fields before using them.

To disable viewer tracking, set ViewerTrackingEnabled to false on the TwitchClient:

```csharp
client.ViewerTrackingEnabled = false;
```

# Using the Message Queue
There is a message queue which can retain IRC messages for later processing automatically. To turn it on when joining a channel, pass 'true' as the second parameter:

```csharp
client.JoinChannel(channelName, true);
```

Alternatively, you can turn message queing on and off later using the StartQueue() and StopQueue() methods of the Channel:

To start queueing messages for a channel:
```csharp
string channelName = ...;
client.Channels[channelName].StartQueue();
```

To stop queueing messages for a joined channel:
```csharp
string channelName = ...;
client.Channels[channelName].StopQueue();
```

To analyze the messages in the message queue, you can get the MessageQueue object from a joined Channel, then get the Queue from the MessageQueue:

```csharp
string channelName = ...;
List<IRCMessage> messageQueue = client.Channels[channelName].MessageQueue.Queue;

foreach(IRCMessage message in messageQueue) 
{
    //Do something with the message
    if(message is PrivateMessage) 
    {
        //Do something only with viewer messages.
    }
}
```

# Loading the Current List of Viewers
The TwitchClient has a method which can be used to pull a list of all viewer names currently in a channel. This method should not be called frequently, otherwise you risk having your access blocked. Do not call this method more than once per minute. 

If you are using the TwitchClient, it is recommended that you only call this method once at application start time to sync the viewers monitored by the client with the live channel. The TwitchClient will automatically track viewers as they enter and leave after that so the method does not need to be called again. 

Calling this method before calling client.Connect() is recommended but not necessary.

```csharp
string channelName = ...;
client.LoadViewerList(channelName);
```
