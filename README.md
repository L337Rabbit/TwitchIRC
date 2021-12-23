# TwitchIRC
C#/.NET API for sending and receiving Twitch IRC chat messages.

# Creating a simple chat client
To get started, you will need to get an OAUTH token from Twitch at https://twitchapps.com/tmi/. See https://dev.twitch.tv/docs/irc/guide for further info.

Do not share your OAUTH token with anyone. If you're doing development on stream, read further down to see ways to keep your token a secret.

```
using okitoki.twitch.irc.client;

string login = "twitch_login_username";
string oauthToken = "twitch_user_token";

TwitchClient client = new TwitchClient();
client.Credentials = new Credentials(login, oauthToken);
client.ActivateAutoReconnect();
client.Connect();
```

Once you have a client, you can then use it to join any Twitch channel (as long as the user you are using isn't banned in that channel):

```
string channelName = "rabbit_xxxx";
client.JoinChannel(channelName);
```

If you want the TwitchClient to queue messages up so that you can process them later, you can set the optional queueMessages paramter to true when you call JoinChannel:

```
client.JoinChannel(channelName, true);
```

# Keeping your token a secret (Method 1)
There are some mechanisms built into this API to help keep your OAUTH token a secret. One method you can use is to encrypt the token to a local file and load it in at runtime. The first time you create a Twitch client, you can tell it to store the credentials (username and OAUTH token). The encrypted credentials are stored in a file called "tc.crd" by default:

```
TwitchClient client = new TwitchClient();
client.Credentials = new Credentials(login, oauthToken);
client.StoreCredentials(myEncryptionPassword);
```

Once you have run the program and stored the credentials, change the code by deleting the lines where credentials are being set and stored and just call LoadCredentials():

```
TwitchClient client = new TwitchClient();
client.LoadCredentials(myEncryptionPassword);
```

You can further increase the security of the token by using a password input box to enter the "myEncryptionPassword" password secretly prior to creating the Twitch client and loading credentials.

# Keeping your token a secret (Method 2)
As an alternative to method 1 avobe, you can create a credentials file and manually add your login username and OAUTH token to it (unencrypted), then load this credential file when your program runs. The file contents should look like the following:



This method is less secure since anyone with access to the credential file has access to your OAUTH token, but may be a bit easier to set up. If you use this approach, make sure not to open the credential file on stream or share it with anyone, including accidentally committing the file to a code repository like Github.

# Handling specific message types
This API supports nearly the entirety of the Twitch IRC message library and is too extensive to cover every possible message type in this README; however, the more common cases are presented below.
