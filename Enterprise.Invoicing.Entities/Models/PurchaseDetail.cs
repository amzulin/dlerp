using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class PurchaseDetail
    {
        public PurchaseDetail()
        {
            this.PurchaseReturnDetails = new List<PurchaseReturnDetail>();
        }

        public int detailSn { get; set; }
        public string purchaseNo { get; set; }
        public string materialNo { get; set; }
        public string requireNo { get; set; }
        public int requireDetailSn { get; set; }
        public double poAmount { get; set; }
        public double poPrice { get; set; }
        public double poRemain { get; set; }
        public double returnAmount { get; set; }
        public string returnNo { get; set; }
        public Nullable<int> returnDetailSn { get; set; }
        public string remark { get; set; }
        public Nullable<System.DateTime> sendDate { get; set; }
        public virtual Material Material { get; set; }
        public virtual Purchase Purchase { get; set; }
        public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
    }
}
