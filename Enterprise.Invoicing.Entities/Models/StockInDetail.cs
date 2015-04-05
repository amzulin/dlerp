using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class StockInDetail
    {
        public StockInDetail()
        {
            this.PurchaseReturnDetails = new List<PurchaseReturnDetail>();
        }

        public int detailSn { get; set; }
        public string stockinNo { get; set; }
        public string materialNo { get; set; }
        public int depotId { get; set; }
        public double inAmount { get; set; }
        public string changeNo { get; set; }
        public string purchaseNo { get; set; }
        public int purchaseDetailSn { get; set; }
        public double returnAmount { get; set; }
        public string remark { get; set; }
        public virtual Depot Depot { get; set; }
        public virtual Material Material { get; set; }
        public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
        public virtual StockIn StockIn { get; set; }
    }
}
