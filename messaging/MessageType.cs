using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.messaging
{
    public enum MessageType
    {
        UNKNOWN, 
        CLEAR_ALL, 
        CLEAR_SINGLE, 
        GLOBAL_USER_STATE, 
        HOST_TARGET, 
        JOIN, 
        NOTICE, 
        PART, 
        PING, 
        PONG, 
        PRIVATE_MESSAGE, 
        RECONNECT, 
        ROOM_STATE, 
        USER_NOTICE, 
        USER_STATE
    }
}
