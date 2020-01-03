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

    }
}
