using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okitoki.twitch.irc.client
{
    public class ViewerChannelInfo
    {
        public int MonthsSubscribed { get; set; } = 0;

        public bool IsMod { get; set; } = false;

        public ViewerType ViewerType { get; set; }

        public List<Badge> Badges { get; set; }

        public ViewerChannelInfo(int monthsSubscribed = 0, bool isMod = false, ViewerType viewerType = ViewerType.VIEWER, List<Badge> badges = null)
        {
            this.MonthsSubscribed = monthsSubscribed;
            this.IsMod = isMod;
            this.ViewerType = viewerType;
            this.Badges = badges;
        }
    }
}
