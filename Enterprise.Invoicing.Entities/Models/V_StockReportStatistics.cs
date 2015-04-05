using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class V_StockReportStatistics
    {
        public string stockMonth { get; set; }
        public string depotName { get; set; }
        public int depotId { get; set; }
        public Nullable<int> materials { get; set; }
        public Nullable<decimal> startAmount { get; set; }
        public Nullable<decimal> startCost { get; set; }
        public Nullable<decimal> inAmount { get; set; }
        public Nullable<decimal> inCost { get; set; }
        public Nullable<decimal> outAmount { get; set; }
        public Nullable<decimal> outCost { get; set; }
        public Nullable<decimal> endAmount { get; set; }
        public Nullable<decimal> endCost { get; set; }
    }
}
