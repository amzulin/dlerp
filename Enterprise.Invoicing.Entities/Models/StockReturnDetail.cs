using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class StockReturnDetail
    {
        public int detailSn { get; set; }
        public string returnNo { get; set; }
        public string stockoutNo { get; set; }
        public int stockoutDetailSn { get; set; }
        public string materialNo { get; set; }
        public int fromDepotId { get; set; }
        public int toDepotId { get; set; }
        public double returnAmount { get; set; }
        public string remark { get; set; }
        public virtual Depot Depot { get; set; }
        public virtual Depot Depot1 { get; set; }
        public virtual Material Material { get; set; }
        public virtual StockOut StockOut { get; set; }
        public virtual StockOutDetail StockOutDetail { get; set; }
        public virtual StockReturn StockReturn { get; set; }
    }
}
