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

namespace GrandChallange.EventWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Query1FrequentController : ControllerBase
    {
        private readonly ILogger<Query1FrequentController> _logger;

        private static ConcurrentDictionary<string, List<long>> InMemoryData { get; set; }
            = new ConcurrentDictionary<string, List<long>>();

        private KeyValuePair<string, List<long>>[] QueryResult { get; set; }

        public static long QueryTime = 0;

        public static long ReqLast = 0;

        private static string TriggeredPickupTime { get; set; }

        private static string TriggeredDropoffTime { get; set; }

        public Query1FrequentController(ILogger<Query1FrequentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Query1Result Get(long reqTimestamp)
        {
            var query = QueryResult;

            return new Query1Result
            {
                Delay = (ReqLast - reqTimestamp).ToString(),
                PickupDatetime = TriggeredPickupTime,
                DropoffDatetime = TriggeredDropoffTime,
                StartCellId1 = (query.Length > 0) ? ExtractLocation(query[0].Key).pick : null,
                StartCellId2 = (query.Length > 1) ? ExtractLocation(query[1].Key).pick : null,
                StartCellId3 = (query.Length > 2) ? ExtractLocation(query[2].Key).pick : null,
                StartCellId4 = (query.Length > 3) ? ExtractLocation(query[3].Key).pick : null,
                StartCellId5 = (query.Length > 4) ? ExtractLocation(query[4].Key).pick : null,
                StartCellId6 = (query.Length > 5) ? ExtractLocation(query[5].Key).pick : null,
                StartCellId7 = (query.Length > 6) ? ExtractLocation(query[6].Key).pick : null,
                StartCellId8 = (query.Length > 7) ? ExtractLocation(query[7].Key).pick : null,
                StartCellId9 = (query.Length > 8) ? ExtractLocation(query[8].Key).pick : null,
                StartCellId10 = (query.Length > 9) ? ExtractLocation(query[9].Key).pick : null,

                EndCellId1 = (query.Length > 0) ? ExtractLocation(query[0].Key).drop : null,
                EndCellId2 = (query.Length > 1) ? ExtractLocation(query[1].Key).drop : null,
                EndCellId3 = (query.Length > 2) ? ExtractLocation(query[2].Key).drop : null,
                EndCellId4 = (query.Length > 3) ? ExtractLocation(query[3].Key).drop : null,
                EndCellId5 = (query.Length > 4) ? ExtractLocation(query[4].Key).drop : null,
                EndCellId6 = (query.Length > 5) ? ExtractLocation(query[5].Key).drop : null,
                EndCellId7 = (query.Length > 6) ? ExtractLocation(query[6].Key).drop : null,
                EndCellId8 = (query.Length > 7) ? ExtractLocation(query[7].Key).drop : null,
                EndCellId9 = (query.Length > 8) ? ExtractLocation(query[8].Key).drop : null,
                EndCellId10 = (query.Length > 9) ? ExtractLocation(query[9].Key).drop : null
            };
        }

        private void UpdateData(long reqTimestamp, long pickTime, long dropTime, string key)
        {
            QueryTime = Math.Max(QueryTime, dropTime) ;
            ReqLast = Math.Max(ReqLast, reqTimestamp);
            var _30minAgo = QueryTime - (30 * 60 * 1000);

            foreach(var item in InMemoryData)
            {
                InMemoryData[item.Key] = InMemoryData[key].Where(y => y > _30minAgo).ToList();
            }

            QueryResult = InMemoryData.ToArray().OrderByDescending(x => x.Value.Count).Take(10).ToArray();

            if(QueryResult.Any(x => x.Key == key))
            {
                TriggeredDropoffTime = dropTime.ToString();
                TriggeredPickupTime = pickTime.ToString();
            }
        }

        public (string pick, string drop) ExtractLocation(string location)
        {
            var split = location.Split("-");
            return (split[0], split[1]);
        }

        public string AggregateLocation((string pick, string drop) location) => $"{location.pick}-{location.drop}";

        [HttpPost]
        public string Post(Wso2Request<Wso2Model> req)
        {
            var aggregatedCells = req.Event.AggregatedCells;
            var timestamp = req.Event.Timestamp;

            if (InMemoryData.ContainsKey(aggregatedCells))
            {
                InMemoryData[aggregatedCells].Add(timestamp);
            }
            else
            {
                InMemoryData[aggregatedCells] = new List<long>() { timestamp };
            }

            UpdateData(timestamp, req.Event.PickupTime, req.Event.DropoffTime, aggregatedCells);

            return "OK";
        }

        public class Wso2Model
        {
            public string AggregatedCells { get; set; }

            public long PickupTime { get; set; }

            public long DropoffTime { get; set; }

            public long Timestamp { get; set; }
        }
    }
}
