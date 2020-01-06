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

namespace GrandChallange.EventWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Query2FrequentController : ControllerBase
    {
        private readonly ILogger<Query2FrequentController> _logger;

        private static Dictionary<string, Wso2Model> InMemoryData { get; }
            = new Dictionary<string, Wso2Model>();

        private KeyValuePair<string, Wso2Model>[] QueryResult { get; set; }

        public static long lastReqTimestamp = 0;

        private static string TriggeredEmptyTaxisupTime { get; set; }

        private static string TriggeredDropoffTime { get; set; }

        public Query2FrequentController(ILogger<Query2FrequentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Query2Result Get(long reqTimestamp)
        {
            var query = QueryResult;

            return new Query2Result
            {
                Delay = (lastReqTimestamp - reqTimestamp).ToString(),
                PickupDatetime = TriggeredEmptyTaxisupTime,
                DropoffDatetime = TriggeredDropoffTime,

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
        }

        private void UpdateData(long reqTimestamp, long EmptyTaxisTime, long dropTime, string key)
        {
            lastReqTimestamp = Math.Max(lastReqTimestamp, reqTimestamp);

            QueryResult = InMemoryData.Take(10).ToArray();

            if (QueryResult.Any(x => x.Key == key))
            {
                TriggeredDropoffTime = dropTime.ToString();
                TriggeredEmptyTaxisupTime = EmptyTaxisTime.ToString();
            }
        }

        [HttpPost]
        public string Post(Wso2Request<Wso2Model> req)
        {
            var ev = req.Event;
            var cell = ev.CellNumber;

            var timestamp = req.Event.Timestamp;

            lastReqTimestamp = Math.Max(lastReqTimestamp, timestamp);

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
    }

}
