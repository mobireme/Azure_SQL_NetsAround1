using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;

namespace NetsAroundMobileApp.DataObjects
{
    public class CellData : EntityData
    {
        public string Id { get; set; }
        public DateTime SaveTime { get; set; }
        public string Text { get; set; }

        public string Location { get; set; }
        public int Strength { get; set; }
        public string UserName { get; set; }
        public string DataConnState { get; set; }
        public string Data_Activity { get; set; }
        public string Net_Type { get; set; }


    }
}