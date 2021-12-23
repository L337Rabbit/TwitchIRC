using okitoki.twitch.irc.messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.client
{
    public class Channel
    {
        public string Name { get; set; }

        public long RoomID { get; set; }

        public bool EmoteOnlyMode { get; set; } = false;

        public bool FollowersOnlyMode { get; set; } = false;

        public bool R9KMode { get; set; } = false;

        public int MessageWaitTime { get; set; }

        public bool SubOnlyMode { get; set; } = false;

        public SortedDictionary<string, ViewerChannelInfo> ViewerInfo { get; } = new SortedDictionary<string, ViewerChannelInfo>();

        public MessageQueue MessageQueue { get; } = new MessageQueue();

        public void AddOrUpdateViewer(string username, int monthsSubscribed = 0, bool isMod = false, List<Badge> badges = null)
        {
            ViewerChannelInfo info = null;

            if (ViewerInfo.ContainsKey(username))
            {
                info = ViewerInfo[username];
            } else
            {
                info = new ViewerChannelInfo();
                ViewerInfo.Add(username, info);
            }

            info.MonthsSubscribed = monthsSubscribed;
            info.IsMod = isMod;
            info.Badges = badges;
        }

        public void AddOrUpdateViewer(string username, ViewerType viewerType)
        {
            ViewerChannelInfo info;
            if (ViewerInfo.ContainsKey(username))
            {
                info = ViewerInfo[username];
            }
            else
            {
                info = new ViewerChannelInfo();
                ViewerInfo.Add(username, info);
            }

            info.ViewerType = viewerType;
        }

        public void RemoveViewer(string username)
        {
            if(ViewerInfo.ContainsKey(username))
            {
                ViewerInfo.Remove(username);
            }
        }

        public void StartQueue()
        {
            MessageQueue.Start();
        }

        public void StopQueue()
        {
            MessageQueue.Stop();
        }

        public void QueueMessage(IRCMessage message)
        {
            MessageQueue.QueueMessage(message);
        }

        public int ViewerCount
        {
            get
            {
                return ViewerInfo.Count;
            }
        }
    }
}
