using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandChallange.Models;
using GrandChallange.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using GrandChallange.EventWebService.Models;
using System.Collections.Concurrent;
using CsvHelper;

namespace GrandChallange.EventWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Query2FrequentController : ControllerBase
    {
        private readonly ILogger<Query2FrequentController> _logger;

        private static readonly object lockObject = new object();

        private static ConcurrentDictionary<string, Wso2Model> InMemoryData { get; }
            = new ConcurrentDictionary<string, Wso2Model>();

        public static Query2Result CurrentQuery = new Query2Result();
        public Query2FrequentController(ILogger<Query2FrequentController> logger)
        {
            _logger = logger;
        }

        private void UpdateData(long reqTimestamp, long EmptyTaxisTime, long dropTime, string key)
        {

            var query = InMemoryData.Take(10).ToArray();


            Query2Result result = query == null
                ? new Query2Result()
                : new Query2Result
                {
                    Delay = (DateTime.Now.GetUnixTime() - reqTimestamp).ToString(),
                    PickupDatetime = dropTime.ToString(),
                    DropoffDatetime = EmptyTaxisTime.ToString(),

                    EmptyTaxiesInCellId1 = (query.Length > 0) ? (query[0].Value).EmptyTaxis : null,
                    EmptyTaxiesInCellId2 = (query.Length > 1) ? (query[1].Value).EmptyTaxis : null,
                    EmptyTaxiesInCellId3 = (query.Length > 2) ? (query[2].Value).EmptyTaxis : null,
                    EmptyTaxiesInCellId4 = (query.Length > 3) ? (query[3].Value).EmptyTaxis : null,
                    EmptyTaxiesInCellId5 = (query.Length > 4) ? (query[4].Value).EmptyTaxis : null,
                    EmptyTaxiesInCellId6 = (query.Length > 5) ? (query[5].Value).EmptyTaxis : null,
                    EmptyTaxiesInCellId7 = (query.Length > 6) ? (query[6].Value).EmptyTaxis : null,
                    EmptyTaxiesInCellId8 = (query.Length > 7) ? (query[7].Value).EmptyTaxis : null,
                    EmptyTaxiesInCellId9 = (query.Length > 8) ? (query[8].Value).EmptyTaxis : null,
                    EmptyTaxiesInCellId10 = (query.Length > 9) ? (query[9].Value).EmptyTaxis : null,

                    MedianProfitInCellId1 = (query.Length > 0) ? (query[0].Value).MedianProfit : null,
                    MedianProfitInCellId2 = (query.Length > 1) ? (query[1].Value).MedianProfit : null,
                    MedianProfitInCellId3 = (query.Length > 2) ? (query[2].Value).MedianProfit : null,
                    MedianProfitInCellId4 = (query.Length > 3) ? (query[3].Value).MedianProfit : null,
                    MedianProfitInCellId5 = (query.Length > 4) ? (query[4].Value).MedianProfit : null,
                    MedianProfitInCellId6 = (query.Length > 5) ? (query[5].Value).MedianProfit : null,
                    MedianProfitInCellId7 = (query.Length > 6) ? (query[6].Value).MedianProfit : null,
                    MedianProfitInCellId8 = (query.Length > 7) ? (query[7].Value).MedianProfit : null,
                    MedianProfitInCellId9 = (query.Length > 8) ? (query[8].Value).MedianProfit : null,
                    MedianProfitInCellId10 = (query.Length > 9) ? (query[9].Value).MedianProfit : null,

                    ProfitabilityOfCell1 = (query.Length > 0) ? (query[0].Value).Profitability : null,
                    ProfitabilityOfCell2 = (query.Length > 1) ? (query[1].Value).Profitability : null,
                    ProfitabilityOfCell3 = (query.Length > 2) ? (query[2].Value).Profitability : null,
                    ProfitabilityOfCell4 = (query.Length > 3) ? (query[3].Value).Profitability : null,
                    ProfitabilityOfCell5 = (query.Length > 4) ? (query[4].Value).Profitability : null,
                    ProfitabilityOfCell6 = (query.Length > 5) ? (query[5].Value).Profitability : null,
                    ProfitabilityOfCell7 = (query.Length > 6) ? (query[6].Value).Profitability : null,
                    ProfitabilityOfCell8 = (query.Length > 7) ? (query[7].Value).Profitability : null,
                    ProfitabilityOfCell9 = (query.Length > 8) ? (query[8].Value).Profitability : null,
                    ProfitabilityOfCell10 = (query.Length > 9) ? (query[9].Value).Profitability : null,

                    ProfitableCellId1 = (query.Length > 0) ? (query[0].Value).CellNumber : null,
                    ProfitableCellId2 = (query.Length > 1) ? (query[1].Value).CellNumber : null,
                    ProfitableCellId3 = (query.Length > 2) ? (query[2].Value).CellNumber : null,
                    ProfitableCellId4 = (query.Length > 3) ? (query[3].Value).CellNumber : null,
                    ProfitableCellId5 = (query.Length > 4) ? (query[4].Value).CellNumber : null,
                    ProfitableCellId6 = (query.Length > 5) ? (query[5].Value).CellNumber : null,
                    ProfitableCellId7 = (query.Length > 6) ? (query[6].Value).CellNumber : null,
                    ProfitableCellId8 = (query.Length > 7) ? (query[7].Value).CellNumber : null,
                    ProfitableCellId9 = (query.Length > 8) ? (query[8].Value).CellNumber : null,
                    ProfitableCellId10 = (query.Length > 9) ? (query[9].Value).CellNumber : null
                };

            lock (lockObject)
            {
                if (IsChanged(result, CurrentQuery))
                {
                    CurrentQuery = result;

                    using var sw = new StringWriter();
                    using var writer = new CsvWriter(sw);
                    writer.WriteRecord(result);
                    writer.Flush();
                    var record = sw.ToString();

                    System.IO.File.AppendAllText("Query2_res.txt", record + "\n");
                }
            }


        }

        [HttpPost]
        public string Post(Wso2Request<Wso2Model> req)
        {
            var ev = req.Event;
            var cell = ev.CellNumber;

            var timestamp = req.Event.Timestamp;

            InMemoryData[cell] = req.Event;

            UpdateData(timestamp, req.Event.PickupTime.Value, req.Event.DropoffTime.Value, cell);

            return "OK";
        }

        public class Wso2Model
        {
            public string CellNumber { get; set; }

            public float? MedianProfit { get; set; }

            public long? EmptyTaxis { get; set; }

            public long? EmptyTaxisupTime { get; set; }

            public long? PickupTime { get; set; }

            public long? DropoffTime { get; set; }

            public float? Profitability { get; set; }

            public long Timestamp { get; set; }
        }

        public bool IsChanged(Query2Result res1, Query2Result res2)
        {
            return
                res1.EmptyTaxiesInCellId1 != res2.EmptyTaxiesInCellId1 ||
                res1.EmptyTaxiesInCellId2 != res2.EmptyTaxiesInCellId2 ||
                res1.EmptyTaxiesInCellId3 != res2.EmptyTaxiesInCellId3 ||
                res1.EmptyTaxiesInCellId4 != res2.EmptyTaxiesInCellId4 ||
                res1.EmptyTaxiesInCellId5 != res2.EmptyTaxiesInCellId5 ||
                res1.EmptyTaxiesInCellId6 != res2.EmptyTaxiesInCellId6 ||
                res1.EmptyTaxiesInCellId7 != res2.EmptyTaxiesInCellId7 ||
                res1.EmptyTaxiesInCellId8 != res2.EmptyTaxiesInCellId8 ||
                res1.EmptyTaxiesInCellId9 != res2.EmptyTaxiesInCellId9 ||
                res1.EmptyTaxiesInCellId10 != res2.EmptyTaxiesInCellId10 ||

                res1.MedianProfitInCellId1 != res2.MedianProfitInCellId1 ||
                res1.MedianProfitInCellId2 != res2.MedianProfitInCellId2 ||
                res1.MedianProfitInCellId3 != res2.MedianProfitInCellId3 ||
                res1.MedianProfitInCellId4 != res2.MedianProfitInCellId4 ||
                res1.MedianProfitInCellId5 != res2.MedianProfitInCellId5 ||
                res1.MedianProfitInCellId6 != res2.MedianProfitInCellId6 ||
                res1.MedianProfitInCellId7 != res2.MedianProfitInCellId7 ||
                res1.MedianProfitInCellId8 != res2.MedianProfitInCellId8 ||
                res1.MedianProfitInCellId9 != res2.MedianProfitInCellId9 ||
                res1.MedianProfitInCellId10 != res2.MedianProfitInCellId10 ||

                res1.ProfitabilityOfCell1 != res2.ProfitabilityOfCell1 ||
                res1.ProfitabilityOfCell2 != res2.ProfitabilityOfCell2 ||
                res1.ProfitabilityOfCell3 != res2.ProfitabilityOfCell3 ||
                res1.ProfitabilityOfCell4 != res2.ProfitabilityOfCell4 ||
                res1.ProfitabilityOfCell5 != res2.ProfitabilityOfCell5 ||
                res1.ProfitabilityOfCell6 != res2.ProfitabilityOfCell6 ||
                res1.ProfitabilityOfCell7 != res2.ProfitabilityOfCell7 ||
                res1.ProfitabilityOfCell8 != res2.ProfitabilityOfCell8 ||
                res1.ProfitabilityOfCell9 != res2.ProfitabilityOfCell9 ||
                res1.ProfitabilityOfCell10 != res2.ProfitabilityOfCell10 ||

                res1.ProfitableCellId1 != res2.ProfitableCellId1 ||
                res1.ProfitableCellId2 != res2.ProfitableCellId2 ||
                res1.ProfitableCellId3 != res2.ProfitableCellId3 ||
                res1.ProfitableCellId4 != res2.ProfitableCellId4 ||
                res1.ProfitableCellId5 != res2.ProfitableCellId5 ||
                res1.ProfitableCellId6 != res2.ProfitableCellId6 ||
                res1.ProfitableCellId7 != res2.ProfitableCellId7 ||
                res1.ProfitableCellId8 != res2.ProfitableCellId8 ||
                res1.ProfitableCellId9 != res2.ProfitableCellId9 ||
                res1.ProfitableCellId10 != res2.ProfitableCellId10;

        }
    }

}
