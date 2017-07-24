using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetsAroundMobileApp.DataObjects
{
    public class WifiScanDetail : EntityData
    {
        public string Id { get; set; }
        public DateTime SaveTime { get; set; }
        public string WifiItemId { get; set; }
        public string SSID { get; set; }
        public string BSSID { get; set; }
        public string Capabilities { get; set; }
        public int Frequency { get; set; }
        public int Level { get; set; }

    }
}