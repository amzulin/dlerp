using System;
using System.Collections.Generic;

namespace Enterprise.Invoicing.Entities.Models
{
    public partial class PurchaseReturn
    {
        public PurchaseReturn()
        {
            this.PurchaseReturnDetails = new List<PurchaseReturnDetail>();
        }

        public string returnNo { get; set; }
        public int depId { get; set; }
        public int staffId { get; set; }
        public int supplierId { get; set; }
        public double returnAmount { get; set; }
        public double returnCost { get; set; }
        public int status { get; set; }
        public bool valid { get; set; }
        public bool isclose { get; set; }
        public string checkStaff { get; set; }
        public string remark { get; set; }
        public System.DateTime createDate { get; set; }
        public string deportStaff { get; set; }
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseReturnDetail> PurchaseReturnDetails { get; set; }
    }
}
