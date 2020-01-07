using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandChallange.EventWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GrandChallange.EventWebService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Query2MedianController : ControllerBase
    {
        public static List<(float profit, long timestamp)> Data { get; set; }
            = new List<(float, long)>();


        [HttpPost]
        public object Post(Wso2Request<Wso2Model> input)
        {
            var ts = input.Event.Now;
            Data = Data.Where(x => x.timestamp > ts - 900000).ToList();

            Data.Add((input.Event.CurrentProfit, input.Event.Now));

            return new
            {
                profit = Median()
            };
        }

        private float Median()
        {
            var ys = Data.OrderBy(x => x.profit).ToList();
            double mid = (ys.Count - 1) / 2.0;
            return (ys[(int)(mid)].profit + ys[(int)(mid + 0.5)].profit) / 2;
        }

        public class Wso2Model
        {
            public float CurrentProfit { get; set; }

            public long Now { get; set; }
        }
    }
}