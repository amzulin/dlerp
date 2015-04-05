using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class PurchaseReturnDetail
    {
        public int detailSn { get; set; }
        public string returnNo { get; set; }
        public string purchaseNo { get; set; }
        public int purchaseDetailSn { get; set; }
        public string stockinNo { get; set; }
        public int stockinDetailSn { get; set; }
        public string materialNo { get; set; }
        public int depotId { get; set; }
        public double returnAmount { get; set; }
        public double buyPrice { get; set; }
        public double returnCost { get; set; }
        public string remark { get; set; }
        public virtual Depot Depot { get; set; }
        public virtual Material Material { get; set; }
        public virtual Purchase Purchase { get; set; }
        public virtual PurchaseDetail PurchaseDetail { get; set; }
        public virtual PurchaseReturn PurchaseReturn { get; set; }
        public virtual StockIn StockIn { get; set; }
        public virtual StockInDetail StockInDetail { get; set; }
    }
}
