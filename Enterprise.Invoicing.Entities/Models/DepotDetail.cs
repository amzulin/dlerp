using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class DepotDetail
    {
        public int detailSn { get; set; }
        public int depotId { get; set; }
        public string materialNo { get; set; }
        public double depotAmount { get; set; }
        public double depotCost { get; set; }
        public double depotSafe { get; set; }
        public decimal price { get; set; }
        public string remark { get; set; }
        public virtual Depot Depot { get; set; }
        public virtual Material Material { get; set; }
    }
}
