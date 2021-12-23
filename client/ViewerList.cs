using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace okitoki.twitch.irc.client
{
    [Serializable]
    public class ViewerList
    {
        [JsonPropertyName("chatter_count")]
        public int ViewerCount { get; set; }

        [JsonPropertyName("chatters")]
        public ViewerGroups ViewerGroups { get; set; }
    }
}
