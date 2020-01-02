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
        public int TripTimeInSecs { get; set; }

        [Index(5)]
        public int TripDistance { get; set; }

        [Index(6)]
        public float PickupLongitude { get; set; }

        [Index(7)]
        public float PickupLatitude { get; set; }

        [Index(8)]
        public float DropoffLongitude { get; set; }

        [Index(9)]
        public float DropoffLatitude { get; set; }

        [Index(10)]
        public string PaymentType { get; set; }

        [Index(11)]
        public float FareAmount { get; set; }

        [Index(12)]
        public float Surcharge { get; set; }

        [Index(13)]
        public float MtaTax { get; set; }

        [Index(14)]
        public float TipAmount { get; set; }

        [Index(15)]
        public float TollsAmount { get; set; }

        [Index(16)]
        public float TotalAmount { get; set; }

    }
}
