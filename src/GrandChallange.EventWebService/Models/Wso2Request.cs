using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandChallange.EventWebService.Models
{
    public class Wso2Request<T>
    {
        public T Event { get; set; }
    }
}
