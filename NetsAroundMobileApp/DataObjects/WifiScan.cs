using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetsAroundMobileApp.DataObjects
{
    public class WifiScan: EntityData
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime SaveTime { get; set; }
        public bool Complete { get; set; }

        public string Location { get; set; }
        public int WifisFound { get; set; }
        public string UserName { get; set; }

    }
}