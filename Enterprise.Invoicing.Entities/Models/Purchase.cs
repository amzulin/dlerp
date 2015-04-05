using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class Purchase
    {
        public Purchase()
        {
            this.PurchaseDetails = new List<PurchaseDetail>();
            this.PurchaseReturnDetails = new List<PurchaseReturnDetail>();
        }

        public string purchaseNo { get; set; }
        public int staffId { get; set; }
        public int depId { get; set; }
        public int supplierId { get; set; }
        public int type { get; set; }
        public double totalAmount { get; set; }
        public double totalCost { get; set; }
        public double totalRemain { get; set; }
        public int status { get; set; }
        public int isover { get; set; }
        public bool isclose { get; set; }
        public System.DateTime createDate { get; set; }
        public bool valid { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
    }
}
