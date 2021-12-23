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
    public class ViewerGroups
    {
        [JsonPropertyName("broadcaster")]
        public List<string> Broadcasters { get; set; }

        [JsonPropertyName("vips")]
        public List<string> Vips { get; set; }

        [JsonPropertyName("moderators")]
        public List<string> Moderators { get; set; }

        [JsonPropertyName("staff")]
        public List<string> Staff { get; set; }

        [JsonPropertyName("admins")]
        public List<string> Admins { get; set; }

        [JsonPropertyName("global_mods")]
        public List<string> GlobalModeratorss { get; set; }

        [JsonPropertyName("viewers")]
        public List<string> Viewers { get; set; }
    }
}
