using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandChallange.EventWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MathNet.Numerics.Statistics;

namespace GrandChallange.EventWebService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Query2MedianController : ControllerBase
    {
        public static object lockObject = new object();

        public static ConcurrentDictionary<float, long> Data { get; set; } = new ConcurrentDictionary<float, long>();

        public static long CurrentTime = 0;

        [HttpPost]
        public object Post(Wso2Request<Wso2Model> input)
        {

            lock (lockObject)
            {
                var ts = input.Event.Now;
                CurrentTime = Math.Max(CurrentTime, ts);

                foreach (var item in Data)
                {
                    if (CurrentTime - 900000 > item.Value)
                        Data.TryRemove(item.Key, out long x);
                }


                Data[input.Event.CurrentProfit] = input.Event.Now;
            }

            return new
            {
                profit = Median()
            };
        }

        private float Median()
        {
            try
            {
                return Data.OrderBy(x => x.Key).Select(x => x.Key).Median();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public class Wso2Model
        {
            public float CurrentProfit { get; set; }

            public long Now { get; set; }
        }
    }
}