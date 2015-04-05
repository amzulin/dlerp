using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Depot
    {
        public Depot()
        {
            this.DepotDetails = new List<DepotDetail>();
            this.PurchaseReturnDetails = new List<PurchaseReturnDetail>();
            this.StockInDetails = new List<StockInDetail>();
            this.StockOutDetails = new List<StockOutDetail>();
            this.StockReturnDetails = new List<StockReturnDetail>();
            this.StockReturnDetails1 = new List<StockReturnDetail>();
        }

        public int depotId { get; set; }
        public string depotName { get; set; }
        public bool valid { get; set; }
        public string remark { get; set; }
        public virtual ICollection<DepotDetail> DepotDetails { get; set; }
        public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
        public virtual ICollection<StockInDetail> StockInDetails { get; set; }
        public virtual ICollection<StockOutDetail> StockOutDetails { get; set; }
        public virtual ICollection<StockReturnDetail> StockReturnDetails { get; set; }
        public virtual ICollection<StockReturnDetail> StockReturnDetails1 { get; set; }
    }
}
