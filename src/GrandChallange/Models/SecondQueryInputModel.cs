using System;
using System.Collections.Generic;
using System.Text;

namespace GrandChallange.Models
{
    class SecondQueryInputModel
    {
        public string Medallion { get; set; }
        public long PickupTime { get; set; }
        public long DropoffTime { get; set; }
        public string PickCell { get; set; }
        public string DropCell { get; set; }
        public long EventTimestamp { get; set; }
        public string PickupTimeOrig { get; set; }
        public string DropoffTimeOrig { get; set; }
        public float FareAmount { get; set; }
        public float TipAmount { get; set; }
    }
}
