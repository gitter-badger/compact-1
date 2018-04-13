using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Compact
{
    [Serializable()]
    public class Bundle
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("banner_url")]
        public string BannerUrl { get; set; }

        [JsonProperty("software")]
        public string[] SoftwareList { get; set; }

        [JsonProperty("installers")]
        public string[] InstallerList { get; set; }
    }
}
