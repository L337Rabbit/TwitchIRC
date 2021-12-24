# TwitchIRC
C#/.NET API for sending and receiving Twitch IRC chat messages.

# Creating a Simple Chat Client
To get started, you will need to get an OAUTH token from Twitch at https://twitchapps.com/tmi/. See https://dev.twitch.tv/docs/irc/guide for further info.

Do not share your OAUTH token with anyone. If you're doing development on stream, read further down to see 2 methods for keeping your token a secret.

```csharp
using okitoki.twitch.irc.client;

string login = "twitch_login_username";
string oauthToken = "twitch_user_token";

TwitchClient client = new TwitchClient();
client.Credentials = new Credentials(login, oauthToken);
client.ActivateAutoReconnect();
client.Connect();
```

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

# Handling specific message types
This API supports nearly the entirety of the Twitch IRC message library and is too extensive to cover every possible message type in this README; however, the more common cases are presented in the **Examples** section below. All messages can be received and relayed to your own methods for processing using the event handlers of the TwitchClient. The various types of message event handlers available are described in the following table:

| Handler Name | Description |
| OnPrivateMessageReceived | Called when a chat message from a specific user comes in. |
| OnSubReceived | Called when a subscription notification comes in. |
| OnGiftedSubReceived | Called when a gifted subscription comes in. |
| OnRaidMessageReceived | Called when a raid notification comes in. |
| OnHostMessageReceived | Called when a host notification comes in. |
| OnUserNoticeReceived | Called when a notice about a specific user in chat comes in. |
| OnUserStateReceived | Called when channel-specific state information about a specific user in chat comes in. |
| OnGlobalUserStateReceived | Called when global state information about a specific user in chat comes in. This |
| OnUnraidMessageReceived | Called when an unraid notification comes in. |
| OnRoomStateReceived | Called when a room state notification comes in. |
| OnJoinMessageReceived | Called when a viewer enters a joined channel. |
| OnPartMessageReceived | Called when a viewer leaves a joined channel. |
| OnNoticeReceived | Called when a NOTICE is received. See https://dev.twitch.tv/docs/irc/msg-id for NOTICE types. |
| OnBitsBadgeTierUpdateReceived | Called when a notification about a viewer's bits badge being upgraded comes in. |
| OnGiftSubUpgradeReceived | Called when a viewer converts from a gifted sub to a paid sub. |
| OnPrimeSubUpgradeReceived | Called when a viewer converts from an Amazon prime sub to a paid sub. |
| OnRitualMessageReceived | Called when a ritual notification is received. Currently this only indicates when a new chatter has joined. |
| OnClearAllMessagesOfUserReceived | Called when a notification has been received indicating that all messages of a specific viewer have been deleted. |
| OnClearSingleUserMessageReceived | Called when a notification has been received indicating that a single message of a specific viewer was deleted. |
| OnReconnectMessageReceived | Called when the Twitch server is indicating to the client to reconnect. |
| OnPingReceived | Called when a ping is received from Twitch servers. |
| OnAnyMessageReceived | Called when any type of message is received by the client. |
| OnChannelSpecificMessageReceived | Called when any type of message specific to a channel is received. |
| OnGeneralUserNoticeReceived | Called when an unrecognized type of USER NOTICE is received. |

## Examples:
### Processing Chat Messages



### Tracking Viewers Entering/Leaving

### Detecting Subscriptions



### Detecting Raids

### Detecting Hosts


# Sending Messages in Chat

# Getting Information About Viewers
The TwtichClient can automatically gather certain information about viewers as they send messages in chat. The amount of information obtained about viewers using this mechanism is limited, but includes the following:

| Property Name | Description |
| --- | --- |
| UserID | The numeric Twitch ID assigned to the user. Example: 401223 |
| Username | The login name of the user. This is identical to display name except all lowercase. |
| DisplayName | The name of the user as displayed in Twitch chat with correct capitalization. |
| NameColor | The hex color code used in chat for this user's name. |
| EmoteSets | A List of IDs for the emote sets which the user has available. |

Additionally, the TwitchClient can gather information about viewers on a per-channel basis. This information includes the following:

| Property Name | Description |
| --- | --- |
| MonthsSubscribed | The number of months the viewer has been subscribed to the channel. |
| IsMod | Whether the viewer is a mod in the channel. |
| ViewerType | The type of viewer (ADMIN, BROADCASTER, GLOBAL_MODERATOR, MODERATOR, STAFF, VIEWER, or VIP). |
| Badges | A collection of Badges applicable to the viewer. Usually displayed to the left of the viewer's name in Twitch chat. |

# Using the Message Queue

# Loading the Current List of Viewers
