using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrandChallange.Models
{
    public class DataModel
    {
        [Index(0)]
        public string Medallion { get; set; }

        [Index(1)]
        public string HackLicense { get; set; }

        [Index(2)]
        public string PickupDatetime { get; set; }

        [Index(3)]
        public string DropoffDatetime { get; set; }

        [Index(4)]
        public string TripTimeInSecs { get; set; }

        [Index(5)]
        public string TripDistance { get; set; }

        [Index(6)]
        public string PickupLongitude { get; set; }

        [Index(7)]
        public string PickupLatitude { get; set; }

        [Index(8)]
        public string DropoffLongitude { get; set; }

        [Index(9)]
        public string DropoffLatitude { get; set; }

        [Index(10)]
        public string PaymentType { get; set; }

        [Index(11)]
        public string FareAmount { get; set; }

        [Index(12)]
        public string Surcharge { get; set; }

        [Index(13)]
        public string MtaTax { get; set; }

        [Index(14)]
        public string TipAmount { get; set; }

        [Index(15)]
        public string TollsAmount { get; set; }

        [Index(16)]
        public string TotalAmount { get; set; }

    }
}
