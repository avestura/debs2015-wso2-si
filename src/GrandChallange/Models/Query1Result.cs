using CsvHelper.Configuration.Attributes;
using System;

namespace GrandChallange.Models
{
    public class Query1Result
    {
        [Index(0)]
        public string PickupDatetime { get; set; }

        [Index(1)]
        public string DropoffDatetime { get; set; }

        [Index(2)]
        public string StartCellId1 { get; set; }

        [Index(3)]
        public string EndCellId1 { get; set; }

        [Index(4)]
        public string StartCellId2 { get; set; }

        [Index(5)]
        public string EndCellId2 { get; set; }

        [Index(6)]
        public string StartCellId3 { get; set; }

        [Index(7)]
        public string EndCellId3 { get; set; }

        [Index(8)]
        public string StartCellId4 { get; set; }

        [Index(9)]
        public string EndCellId4 { get; set; }

        [Index(10)]
        public string StartCellId5 { get; set; }

        [Index(11)]
        public string EndCellId5 { get; set; }

        [Index(12)]
        public string StartCellId6 { get; set; }

        [Index(13)]
        public string EndCellId6 { get; set; }

        [Index(14)]
        public string StartCellId7 { get; set; }

        [Index(15)]
        public string EndCellId7 { get; set; }

        [Index(16)]
        public string StartCellId8 { get; set; }

        [Index(17)]
        public string EndCellId8 { get; set; }

        [Index(18)]
        public string StartCellId9 { get; set; }

        [Index(19)]
        public string EndCellId9 { get; set; }

        [Index(20)]
        public string StartCellId10 { get; set; }

        [Index(21)]
        public string EndCellId10 { get; set; }

        [Index(22)]
        public string Delay { get; set; }

        public override string ToString()
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime pickupTime = start.AddMilliseconds(long.Parse(PickupDatetime)).ToLocalTime();
            DateTime droffTimedate = start.AddMilliseconds(long.Parse(DropoffDatetime)).ToLocalTime();

            return "Pickup datetime: " + pickupTime.ToString() + "\n" +
                "Dropoof datetime: " + droffTimedate.ToString() + "\n" +
                "No.1 Start cell id: " + StartCellId1 + " End cell id:" + EndCellId1 + "\n" +
                "No.2 Start cell id: " + StartCellId2 + " End cell id:" + EndCellId2 + "\n" +
                "No.3 Start cell id: " + StartCellId3 + " End cell id:" + EndCellId3 + "\n" +
                "No.4 Start cell id: " + StartCellId4 + " End cell id:" + EndCellId4 + "\n" +
                "No.5 Start cell id: " + StartCellId5 + " End cell id:" + EndCellId5 + "\n" +
                "No.6 Start cell id: " + StartCellId6 + " End cell id:" + EndCellId6 + "\n" +
                "No.7 Start cell id: " + StartCellId7 + " End cell id:" + EndCellId7 + "\n" +
                "No.8 Start cell id: " + StartCellId8 + " End cell id:" + EndCellId8 + "\n" +
                "No.9 Start cell id: " + StartCellId9 + " End cell id:" + EndCellId9 + "\n" +
                "No.10 Start cell id: " + StartCellId10 + " End cell id:" + EndCellId10 + "\n" +
                "Delay: " + Delay + " ms\n";
        }

    }
}
