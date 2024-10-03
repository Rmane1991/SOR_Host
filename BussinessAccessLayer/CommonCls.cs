using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessAccessLayer
{

    public class ParentOrder
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("order")]
        public string Order { get; set; }
    }

    public class ChildOrder
    {
        [JsonProperty("ruleid")]
        public string RuleId { get; set; }

        [JsonProperty("order")]
        public string Order { get; set; }
    }

    public class OrderData
    {
        public List<ParentOrder> ParentOrder { get; set; }
        public Dictionary<string, List<ChildOrder>> ChildOrders { get; set; } 
    }


}
