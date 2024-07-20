using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VrBrowserTestCore
{
    public class AppSettings
    {
        public float Size { get; set; } = 0.9f;
        public float Offset { get; set; } = 0.45f;
        public float Curvature { get; set; } = 0.1f;
        public int ColorFormat { get; set; } = 0;

        public List<Tab> Tabs { get; set; } = new List<Tab>();
    }

    public class Tab
    {
        public string Url { get; set; } = "";
        public string Css { get; set; } = "";
        public bool AudioEnabled { get; set; } = false;

        public Guid Tag { get; set; } = Guid.NewGuid();

        [IgnoreDataMember]
        [JsonIgnore]
        public ChromiumWebBrowser? Browser { get; set; }
        [IgnoreDataMember]
        [JsonIgnore]
        public bool UrlLoaded { get; set; }
    }
}
